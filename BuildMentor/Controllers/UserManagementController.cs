using BuildMentor.Database.Entities;
using BuildMentor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BuildMentor.Controllers
{
    [Authorize(Roles="ADMIN")]
    public class UserManagementController : Controller
    {
        private readonly UnitService unitService;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole<int>> roleManager;

        public UserManagementController(UnitService unitService, UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            this.unitService = unitService;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet("/Admin/Users")]
        public async Task<IActionResult> Index()
        {
            var id = int.Parse(userManager.GetUserId(User));
            var users = await unitService.UserService.GetUsersAsync(userManager);
            return View("/Views/Admin/Users/Index.cshtml", users.ToList());
        }

        [HttpPost("/Admin/Users/Remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var user = unitService.UserService.Get(id);
            var admin = await userManager.GetUserAsync(User);
            foreach(var tool in user.UserTools)
            {
                unitService.UserToolService.Delete(tool.Id);
            }
            var toolPermissionRequests = unitService.ToolPermissionRequestService.GetAll().Where(x => x.UserId == id);
            foreach(var request in toolPermissionRequests)
            {
                unitService.ToolPermissionRequestService.Delete(request.Id);
            }
            var adminRequests = unitService.AdminRequestService.GetAll().Where(x => x.SenderId == id);
            foreach(var request in adminRequests)
            {
                unitService.AdminRequestService.Delete(request.Id);
            }

            userManager.RemoveFromRoleAsync(user, "USER");
            Task.Run(async () =>
            {
                await unitService.SmtpService.UserRemovedAsync(user, admin.Email);
                });

            unitService.UserService.Delete(id);
            
            return Ok(new { message = "Success" });
        }
    }
}
