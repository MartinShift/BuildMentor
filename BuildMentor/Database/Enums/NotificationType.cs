using Microsoft.AspNetCore.Mvc;

namespace BuildMentor.Database.Enums
{
    public enum NotificationType 
    {
        ToolNeedsMaintenance,
        ToolUnderMaintenance,
        MantenaceCompleted,
        ToolPermissionRemoved,
        ToolPermissionRequestDenied,
        ToolPermissionRequestApproved,
        AdminRequestDenied,
    }
}
