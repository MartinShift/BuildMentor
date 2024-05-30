using BuildMentor.Database.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildMentor.Database.Entities
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }

        public string Address { get; set; } 

        public string City { get; set; }

        public string Country { get; set; }

        public DateOnly BirthDate { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public BuilderJobs Job { get; set; }
        public virtual Image? Avatar {  get; set; }
       
        [InverseProperty(nameof(Notification.Admin))]
        public virtual IList<Notification> ReceivedNotifications { get; set; }

        [InverseProperty(nameof(ToolPermission.User))]
        public virtual IList<ToolPermission> ToolPermissions { get; set; }

        [InverseProperty(nameof(ToolPermission.Admin))]
        public virtual IList<ToolPermission> GivenPermissions { get; set; }

        [InverseProperty(nameof(ToolPermissionRequest.User))]
        public virtual IList<ToolPermissionRequest> ToolPermissionRequests { get; set; }

        [InverseProperty(nameof(UserTool.User))]
        public virtual IList<UserTool> UserTools { get; set; }
    }
}
