using BuildMentor.Models;
using BuildMentor.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BuildMentor.Database.Entities;
using Microsoft.AspNetCore.Authorization;
namespace BuildMentor.Controllers
{
    [Authorize(Roles="ADMIN")]
    public class AdminController : Controller
    {
        private readonly UnitService unitService;

        private readonly UserManager<User> userManager;

        public AdminController(UnitService unitService, UserManager<User> userManager)
        {
            this.unitService = unitService;
            this.userManager = userManager;
        }

        [HttpGet("/Admin")]
        public IActionResult Index()
        {
            var id = int.Parse(userManager.GetUserId(User));
            var users = unitService.UserService.GetAll();
            var model = new AdminDashboardModel()
            {
                NewNotifications = unitService.NotificationService.GetAll().Count(x => x.AdminId == id && x.CreatedDate >= DateTime.Now - TimeSpan.FromDays(1)),
                PendingToolPermissions = unitService.ToolPermissionRequestService.GetAll().Count(),
                PendingAdminPermissions = unitService.AdminRequestService.GetAll().Count(),
                NewUsers = unitService.UserService.GetAll().Count(x => x.CreatedAt >= DateTime.Now - TimeSpan.FromDays(1)),
            };
            return View(model);
        }
    }
}
