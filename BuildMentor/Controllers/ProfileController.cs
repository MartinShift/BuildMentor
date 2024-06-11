using BuildMentor.Database;
using BuildMentor.Database.Entities;
using BuildMentor.Models;
using BuildMentor.Resources;
using BuildMentor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BuildMentor.Controllers
{
    [Controller]
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly BuildContext _context;
        private readonly UserService _userService;
        private readonly ImageService _imageService;
        private readonly SmtpService _smtpService;

        public ProfileController(UserManager<User> userManager, BuildContext context, UserService userService, ImageService imageService, SmtpService smtpService)
        {
            _userManager = userManager;
            _context = context;
            _userService = userService;
            _imageService = imageService;
            _smtpService = smtpService;
        }
        [HttpGet]
        public async Task<IActionResult> UserProfile(int id)
        {
            var user = _userService.Get(id);
            var profile = _userService.MapToProfile(user, await _userManager.IsInRoleAsync(user, "ADMIN"));
            return View("Profile", profile);
        }

        [HttpGet("/Profile/ViewProfile/{userId}")]
        public async Task<IActionResult> ViewProfile(int userId)
        {
            return RedirectToAction("UserProfile", new { id = userId });
        }

        [Authorize]
        public async Task<IActionResult> MyProfile()
        {
            var usr = await _userManager.GetUserAsync(User);
            return RedirectToAction("UserProfile", new { id = usr.Id });
        }

        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var usr = await _userManager.GetUserAsync(User);
            var user = _context.Users.Include(x => x.Avatar).First(x => x.UserName == usr.UserName);
            var userProfile = _userService.MapToProfile(user, User.IsInRole("ADMIN"));
            return View(userProfile);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile([FromForm] UserProfile model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                return BadRequest(new { Error = Resource.ResourceManager.GetString("Name is required.") });
            }
            if (string.IsNullOrEmpty(model.Email))
            {
                return BadRequest(new { Error = Resource.ResourceManager.GetString("Email is required.") });
            }
            if (string.IsNullOrEmpty(model.PhoneNumber))
            {
                return BadRequest(new { Error = Resource.ResourceManager.GetString("Phone Number is required.") });
            }
            if (string.IsNullOrEmpty(model.City))
            {
                return BadRequest(new { Error = Resource.ResourceManager.GetString("City is required.") });
            }
            if (string.IsNullOrEmpty(model.Country))
            {
                return BadRequest(new { Error = Resource.ResourceManager.GetString("Country is required.") });
            }
            if (string.IsNullOrEmpty(model.Address))
            {
                return BadRequest(new { Error = Resource.ResourceManager.GetString("Address is required.") });
            }
            if (model.BirthDate == null)
            {
                return BadRequest(new { Error = Resource.ResourceManager.GetString("Birth Date is required.") });
            }
            if (DateTime.Now.Year - model.BirthDate.Year < 16)
            {
                return BadRequest(new { Error = Resource.ResourceManager.GetString("You must be at least 16 years old.") });
            }
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                user.Name = model.Name;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.Job = model.Job;
                user.PhoneNumber = model.PhoneNumber;
                user.City = model.City;
                user.BirthDate = DateOnly.FromDateTime(model.BirthDate) >= new DateOnly(1900, 1, 1) ? DateOnly.FromDateTime(model.BirthDate) : user.BirthDate;
                user.Country = model.Country;
                user.Address = model.Address;

                if (model.UploadedAvatar != null)
                {
                    var avatar = await _imageService.Upload(model.UploadedAvatar);
                    user.Avatar = avatar;
                }

             

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {  
                await Task.Run(async () =>
                {
                 await _smtpService.ChangeEmailAsync(user);
                 });
                    return Ok(new { Message = "Success!" });
                }
                else
                {
                    return BadRequest(new { Error = "Failed to update profile", Errors = result.Errors });
                }
            }
            else
            {
                return NotFound(new { Message = "User not found" });
            }
        }
    }
}
