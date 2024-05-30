using BuildMentor.Database.Entities;
using BuildMentor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BuildMentor.Controllers
{
    [Authorize(Roles="ADMIN")]
    public class PermissionsController : Controller
    {
        private readonly UnitService unitService;
        private readonly UserManager<User> userManager;

        public PermissionsController(UnitService unitService, UserManager<User> userManager)
        {
            this.unitService = unitService;
            this.userManager = userManager;
        }

        [HttpGet("/Admin/Permissions")]
        public IActionResult Index()
        {
            var id = int.Parse(userManager.GetUserId(User));
            var permissions = unitService.ToolPermissionService.GetAll().Where(x=> x.AdminId == id);
            return View("/Views/Admin/Permissions/Index.cshtml", permissions);
        }

        [HttpPost("/Admin/Permissions/Remove/{id}")]
        public IActionResult Remove(int id, [FromBody] string message)
        {
            var permission = unitService.ToolPermissionService.Get(id);
            Task.Run(async () => {
                await unitService.SmtpService.PermissionRemovedAsync(permission,message);
                });

            var notification = new UserNotification
            {
                Comment = message,
                UserId = permission.UserId,
                AdminId = permission.AdminId,
                Type = Database.Enums.NotificationType.ToolPermissionRemoved,
                CreatedDate = DateTime.Now,
            };
            unitService.UserNotificationService.Add(notification);
            unitService.ToolPermissionService.Delete(id);
            return Ok(new { message = "Success" });
        }

    }
}
