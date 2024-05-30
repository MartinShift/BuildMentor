using BuildMentor.Database.Entities;
using BuildMentor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BuildMentor.Controllers
{
    [Authorize(Roles="ADMIN")]
    public class NotificationsController : Controller
    {
        private readonly UnitService unitService;
        private readonly UserManager<User> userManager;

        public NotificationsController(UnitService unitService, UserManager<User> userManager)
        {
            this.unitService = unitService;
            this.userManager = userManager;
        }

        [HttpGet("/Admin/Notifications/")]
        public IActionResult Index()
        {
            var id = int.Parse(userManager.GetUserId(User));
            var notifications = unitService.NotificationService.GetAll().Where(x=> x.AdminId == id);
            return View("/Views/Admin/Notifications/Index.cshtml", notifications);
        }

        [HttpGet("/Admin/Notifications/Download/{id}")]
        public async Task<IActionResult> Download(int id)
        {
            var notification = unitService.NotificationService.Get(id);

            if (notification == null)
            {
                return NotFound();
            }

            var document = unitService.UserToolService.Get(notification.UserToolId).ToolMaintenanceRecords.Last().Document;

            if (!System.IO.File.Exists(document.Path()))
            {
                return NotFound();
            }

            var bytes = await System.IO.File.ReadAllBytesAsync(document.Path());

            return File(bytes, "application/octet-stream", document.Filename);

        }

        [HttpGet("/Admin/Notifications/Latest/")]
        public IActionResult Latest()
        {
            var id = int.Parse(userManager.GetUserId(User));
            var notifications = unitService.NotificationService.GetAll().Where(x => x.AdminId == id).OrderByDescending(x => x.CreatedDate).Take(5);
            return Ok(new { Notifications = notifications });
        }

        [HttpGet("/Notifications/Remove/User/{id}")]
        public IActionResult RemoveUser(int id)
        {
            var notification = unitService.UserNotificationService.Get(id);

            if (notification == null)
            {
                return NotFound();
            }

            unitService.UserNotificationService.Delete(notification);
            return Redirect("/User/Notifications");
        }

        [HttpGet("/Notifications/Remove/Admin/{id}")]
        public IActionResult RemoveAdmin(int id)
        {
            var notification = unitService.NotificationService.Get(id);

            if (notification == null)
            {
                return NotFound();
            }

            unitService.NotificationService.Delete(notification);
            return Redirect("/Admin/Notifications");
        }


    }
}
