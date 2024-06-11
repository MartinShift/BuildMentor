using BuildMentor.Database.Entities;
using BuildMentor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BuildMentor.Controllers
{
    [Authorize(Roles ="ADMIN")]
    public class AdminRequestController : Controller
    {
        private readonly UnitService unitService;
        private readonly UserManager<User> userManager;

        public AdminRequestController(UnitService unitService, UserManager<User> userManager)
        {
            this.unitService = unitService;
            this.userManager = userManager;
        }

        [HttpGet("/Admin/AdminRequests/")]
        public IActionResult Index()
        {
            var requests = unitService.AdminRequestService.GetAll();
            return View("/Views/Admin/AdminRequests/Index.cshtml",requests);
        }

        [HttpPost("/Admin/AdminRequests/Approve/{id}")]
        public async Task<IActionResult> Approve(int id, [FromBody] string comment)
        {
            var request = unitService.AdminRequestService.Get(id);

            var user = request.Sender;

            await userManager.AddToRoleAsync(user, "ADMIN");
            await userManager.RemoveFromRoleAsync(user, "USER");

            await Task.Run(async () =>
            {
                await unitService.SmtpService.AdminRequestApprovedAsync(user, comment);
            });
            unitService.AdminRequestService.Delete(request);

            return Ok(new { Message = "Success" });
        }

        [HttpPost("/Admin/AdminRequests/Reject/{id}")]
        public async Task<IActionResult> Reject(int id, [FromBody] string comment)
        {
            var request = unitService.AdminRequestService.Get(id);

            unitService.AdminRequestService.Delete(request);

            await Task.Run(async () =>
            {
                await unitService.SmtpService.AdminRequestRejectedAsync(request.Sender, comment);
            });
            var notfication = new UserNotification
            {
                Comment = comment,
                UserId = request.SenderId,
                AdminId = int.Parse(userManager.GetUserId(User)),
                Type = Database.Enums.NotificationType.AdminRequestDenied,
                CreatedDate = DateTime.Now,
            };
            unitService.UserNotificationService.Add(notfication);
            return Ok(new { Message = "Success" });
        }

    }
}
