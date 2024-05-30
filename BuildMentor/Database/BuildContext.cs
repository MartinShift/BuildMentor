using BuildMentor.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BuildMentor.Database
{
    public class BuildContext : IdentityDbContext<User,IdentityRole<int>,int>
    {
        public BuildContext(DbContextOptions<BuildContext> options) : base(options)
        {
        }

        public virtual DbSet<Image> Images { get; set; }

        public virtual DbSet<Instruction> Instructions { get; set; }

        public virtual DbSet<Notification> Notifications { get; set; }

        public virtual DbSet<Tool> Tools { get; set; }

        public virtual DbSet<ToolMaintenanceRecord> ToolMaintenanceRecords { get; set; }

        public virtual DbSet<ToolPermission> ToolPermissions { get; set; }

        public virtual DbSet<ToolPermissionRequest> ToolPermissionRequests { get; set; }

        public virtual DbSet<UserTool> UserTools { get; set; }

        public virtual DbSet<ActiveMaintenance> ActiveMaintenances { get; set; }

        public virtual DbSet<AdminRequest> AdminRequests { get; set; }

        public virtual DbSet<UserNotification> UserNotifications { get; set; }
    }
}
