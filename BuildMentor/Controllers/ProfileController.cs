using BuildMentor.Database;
using BuildMentor.Database.Entities;
using BuildMentor.Models;
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

            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                user.Name = model.Name;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.Job = model.Job;
                user.PhoneNumber = model.PhoneNumber;
                user.City = model.City;
                user.BirthDate = model.BirthDate >= new DateOnly(1900, 1, 1) ? model.BirthDate : user.BirthDate;
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
