using BuildMentor.Database;
using BuildMentor.Database.Entities;
using BuildMentor.Models;
using BuildMentor.Resources;
using BuildMentor.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace BuildMentor.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly UnitService service;
        private readonly IConfiguration configuration;
        private readonly DataSeed _dataSeed;

        public AccountController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            UnitService unitService,
            IConfiguration configuration,
            DataSeed dataSeed
            )
        {
            this.configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            service = unitService;
            _dataSeed = dataSeed;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        public async Task SignInClaims(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>();
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            var userIdentity = new ClaimsIdentity(claims, "login");
            var principal = new ClaimsPrincipal(userIdentity);
            await HttpContext.SignInAsync(principal);

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Error = string.Join(",", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()) });
            }
            var existingLogin = await _userManager.FindByNameAsync(model.Login);
            if (existingLogin != null)
            {
                return BadRequest(new { Error = Resource.ResourceManager.GetString("The login is already in use.") });
            }

            var existingEmail = await _userManager.FindByEmailAsync(model.Email);
            if (existingEmail != null)
            {
                return BadRequest(new { Error = Resource.ResourceManager.GetString("The email is already in use.") });
            }

            var user = new User
            {
                Name = model.Name,
                UserName = model.Login,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                City = model.City,
                Country = model.Country,
                Address = model.Address,
                Job = model.Job,
            };

            await Task.Run(async () =>
            {
                await service.SmtpService.WelcomeEmail(user);
            });

            if (model.UploadedAvatar != null)
            {
                var avatar = await service.ImageService.Upload(model.UploadedAvatar);
                user.Avatar = avatar;
            }
            else
            {
                user.Avatar = null;
            }

            var result = await _userManager.CreateAsync(user, model.Password);

            try
            {
                await _userManager.AddToRoleAsync(user, "USER");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


            if (result.Succeeded)
            {
                return Ok(new { Message = "Success!", Role = (await _userManager.GetRolesAsync(user)).First() });
            }


            return BadRequest(new { Error = string.Join(",", result.Errors.Select(e => e.Description).ToList()) });
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            if(!User.Identity.IsAuthenticated)
            {
                //await _dataSeed.SeedData();
                return View();
            }
            if(User.IsInRole("ADMIN"))
            {
                return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("Index", "User");
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = ModelState.ErrorCount, Error = Resource.ResourceManager.GetString("Invalid Model State") });
            }
            var existingEmail = await _userManager.FindByEmailAsync(model.LoginOrEmail);
            var existingLogin = await _userManager.FindByNameAsync(model.LoginOrEmail);
            if (existingLogin == null && existingEmail == null)
            {
                return BadRequest(new { Message = "", Error = Resource.ResourceManager.GetString("No Such Login Exists") });
            }
            if (existingEmail != null) { model.LoginOrEmail = existingEmail.UserName; }

            var result = await _signInManager.PasswordSignInAsync(model.LoginOrEmail, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return BadRequest(new { Message = "", Error = Resource.ResourceManager.GetString("Wrong Password!") });
            }
            var username = existingLogin ?? existingEmail;
            await SignInClaims(username);

            return Ok(new { Message = "Success!", admin = await _userManager.IsInRoleAsync(username, "ADMIN") });
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet("/Account/AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
