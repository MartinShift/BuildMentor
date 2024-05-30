using BuildMentor.Database;
using BuildMentor.Database.Entities;

namespace BuildMentor.Services
{
    public class UnitService
    {
        public readonly UserService UserService;

        public readonly SmtpService SmtpService;

        public readonly ImageService ImageService;

        public readonly ToolService ToolService;

        public readonly InstructionService InstructionService;

        public readonly NotificationService NotificationService;

        public readonly ToolMaintenanceService ToolMaintenanceService;

        public readonly ToolPermissionRequestService ToolPermissionRequestService;

        public readonly ToolPermissionService ToolPermissionService;

        public readonly BuildContext Context;

        public readonly UserToolService UserToolService;

        public readonly ActiveMaintenanceService ActiveMaintenanceService;

        public readonly AdminRequestService AdminRequestService;

        public readonly UserNotificationService UserNotificationService;

        public UnitService(BuildContext context, UserService userService, SmtpService smtpService, ImageService imageService, 
            ToolService toolService, InstructionService instructionService, NotificationService notificationService, ToolMaintenanceService toolMaintenanceService, 
            ToolPermissionRequestService toolPermissionRequestService, ToolPermissionService toolPermissionService, UserToolService userToolService, ActiveMaintenanceService activeMaintenanceService, 
            AdminRequestService adminRequestService, UserNotificationService userNotificationService)
        {
            this.ActiveMaintenanceService = activeMaintenanceService;
            this.UserService = userService;
            this.SmtpService = smtpService;
            this.ImageService = imageService;
            this.ToolService = toolService;
            this.InstructionService = instructionService;
            this.NotificationService = notificationService;
            this.ToolMaintenanceService = toolMaintenanceService;
            this.ToolPermissionRequestService = toolPermissionRequestService;
            this.ToolPermissionService = toolPermissionService;
            this.Context = context;
            this.UserToolService = userToolService;
            this.AdminRequestService = adminRequestService;
            this.UserNotificationService = userNotificationService;
        }
    }
}
