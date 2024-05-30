using Microsoft.AspNetCore.Mvc;
using BuildMentor.Services;
using BuildMentor.Database.Entities;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BuildMentor.Controllers.Admin
{
    [Route("/Admin/PermissionRequests")]
    [Authorize(Roles = "ADMIN")]

    public class PermissionRequestsController : Controller
    {
        private readonly UnitService _unitService;

        private readonly UserManager<User> _userManager;
        public PermissionRequestsController(UnitService unitService, UserManager<User> userManager)
        {
            _unitService = unitService;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            var permissionRequests = _unitService.ToolPermissionRequestService.GetAll();
            return View("/Views/Admin/PermissionRequests/Index.cshtml",permissionRequests);
        }

        [HttpPost]
        [Route("Approve/{id}")]
        public async Task<IActionResult> Approve(int id, [FromBody] string message)
        {
            var request = _unitService.ToolPermissionRequestService.Get(id);
            var adminId = int.Parse(_userManager.GetUserId(User));
            var permission = new ToolPermission()
            {
                ToolId = request.ToolId,
                UserId = request.UserId,
                AdminId = adminId,
            };
            _unitService.ToolPermissionService.Add(permission);
            var userTool = new UserTool()
            {
                ToolId = request.ToolId,
                UserId = request.UserId,
                Condition = Database.Enums.Condition.Working,
                LastMaintenanceDate = System.DateTime.Now,
                WarrantyExpirationDate = System.DateTime.Now.AddYears(1),
                PermissionId = permission.Id,
            };
            _unitService.UserToolService.Add(userTool);

            var notification = new UserNotification
            {
                Comment = message,
                UserId = request.UserId,
                AdminId = adminId,
                Type = Database.Enums.NotificationType.ToolPermissionRequestApproved,
                CreatedDate = DateTime.Now,
            };
            _unitService.UserNotificationService.Add(notification);
            await Task.Run(async () =>
            {
                await _unitService.SmtpService.RequestApprovedAsync(request, message);
            });
            _unitService.ToolPermissionRequestService.Delete(id);
            return Ok(new { Message = "Success!" });
        }

        [HttpPost]
        [Route("Deny/{id}")]
        public async Task<IActionResult> Deny(int id, [FromBody] string message)
        {
            var request = _unitService.ToolPermissionRequestService.Get(id);
            await Task.Run(async () =>
            {
                await _unitService.SmtpService.RequestDeniedAsync(request, message);
            });
            var notification = new UserNotification
            {
                Comment = message,
                UserId = request.UserId,
                AdminId = int.Parse(_userManager.GetUserId(User)),
                Type = Database.Enums.NotificationType.ToolPermissionRequestDenied,
                CreatedDate = DateTime.Now,
            };
            _unitService.UserNotificationService.Add(notification);
            _unitService.ToolPermissionRequestService.Delete(id);
            return Ok();
        }
    }
}
