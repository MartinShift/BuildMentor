using BuildMentor.Database.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildMentor.Database.Entities
{
    public class UserTool
    {
        [Key]
        public int Id { get; set; }

        public DateTime WarrantyExpirationDate { get; set; }

        public Condition Condition { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public User User { get; set; }

        [ForeignKey(nameof(Tool))]
        public int ToolId { get; set; }

        public Tool Tool { get; set; }

        [ForeignKey(nameof(Permission))]
        public int PermissionId { get; set; }

        public ToolPermission Permission { get; set; }

        public DateTime LastMaintenanceDate { get; set; }

        [InverseProperty(nameof(ToolMaintenanceRecord.Tool))]
        public IList<ToolMaintenanceRecord> ToolMaintenanceRecords { get; set; }

        public IList<Notification> Notifications { get; set; }
    }
}
