using BuildMentor.Database.Entities;
using BuildMentor.Database.Enums;
using BuildMentor.Models;
using BuildMentor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BuildMentor.Controllers
{
    [Authorize(Roles = "USER")]
    public class MaintenanceController : Controller
    {
        private readonly UnitService unitService;

        public MaintenanceController(UnitService unitService)
        {
            this.unitService = unitService;
        }

        [HttpGet]
        [Route("/Maintenance/{id}")]
        public IActionResult Index(int id)
        {
            var userTool = unitService.UserToolService.Get(id);
            return View(userTool);
        }

        [HttpPost]
        [Route("/Maintenance/Send/{id}")]
        public async Task<IActionResult> Send(int id, [FromBody] MaintenanceRequestModel model)
        {
            var userTool = unitService.UserToolService.Get(id);
            userTool.Condition = Condition.UnderMaintenance;
            var maintenance = new ActiveMaintenance
            {
                Comment = model.Message,
                UserToolId = userTool.Id,
                Price = model.Price,
                UserTool = userTool,
            };  
            var notification = new Notification
            {
                UserToolId = userTool.Id,
                CreatedDate = DateTime.Now,
                AdminId = userTool.Permission.AdminId,
                Type = NotificationType.ToolUnderMaintenance,
                Comment = model.Message,
            };
            unitService.NotificationService.Add(notification);
            unitService.ActiveMaintenanceService.Add(maintenance);
            _ = Task.Run(async () =>
            {
                await unitService.SmtpService.NotifyAdminUnderMaintenanceAsync(notification);
            });
            unitService.UserToolService.Update(userTool);
            return Ok(new { Message = "Maintenance request sent" });
        }

        [HttpPost]
        [Route("/Maintenance/Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] int condition)
        {
            var userTool = unitService.UserToolService.Get(id);

            userTool.Condition = (Condition)condition;

            unitService.UserToolService.Update(userTool);

            if(userTool.Condition == Condition.NeedsMaintenance || userTool.Condition == Condition.NeedsReplacement)
            {
                var notification = new Notification
                {
                    UserTool = userTool,
                    CreatedDate = DateTime.Now,
                    AdminId = userTool.Permission.AdminId,
                    Type = NotificationType.ToolNeedsMaintenance,
                    Comment = "Tool needs maintenance",
                    UserToolId = userTool.Id,
                };
                unitService.NotificationService.Add(notification);
                _ = Task.Run(async () =>
                {
                    await unitService.SmtpService.NotifyAdminNeedsMaintenanceAsync(notification);
                });
            }

            return Ok(new { Message = "Maintenance request updated" });
        }

        [HttpPost]
        [Route("/Maintenance/Complete/{id}")]
        public async Task<IActionResult> Complete(int id, [FromForm] IFormFile file)
        {
            var userTool = unitService.UserToolService.Get(id);
            var maintenance = unitService.ActiveMaintenanceService.GetAll().First(x => x.UserToolId == id);
            userTool.Condition = Condition.Working;
            var record = new ToolMaintenanceRecord
            {
                ToolId = userTool.Id,
                Price = maintenance.Price, 
                CreatedDate = DateTime.Now,
                Document = await unitService.ImageService.Upload(file),
            };
            userTool.LastMaintenanceDate = DateTime.Now;
            unitService.ActiveMaintenanceService.Delete(maintenance);
            var notification = new Notification
            {
                UserTool = userTool,
                CreatedDate = DateTime.Now,
                AdminId = userTool.Permission.AdminId,
                Type = NotificationType.MantenaceCompleted,
                Comment = "Maintenance complete",
            };
            unitService.NotificationService.Add(notification);
            unitService.ToolMaintenanceService.Add(record);
            unitService.UserToolService.Update(userTool);
            _ = Task.Run(async () =>
            {
                await unitService.SmtpService.NotifyAdminMaintenanceCompletedAsync(notification);
            });
            return Ok(new { Message = "Maintenance completed" });
        }
    }
}
