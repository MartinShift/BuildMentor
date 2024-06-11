using BuildMentor.Database.Entities;
using BuildMentor.Database.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace BuildMentor.Models
{
    public class UserProfile
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string AvatarUrl { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Address { get; set; }

        public bool IsAdmin { get; set; }

        public BuilderJobs Job { get; set; }

        public DateTime BirthDate { get; set; }

        public IFormFile? UploadedAvatar { get; set; }

        public IList<Notification> SentNotifications { get; set; }

        public IList<Notification> ReceivedNotifications { get; set; }

        public IList<ToolPermission> ToolPermissions { get; set; }

        public IList<ToolPermission> GivenPermissions { get; set; }

        public IList<ToolPermissionRequest> ToolPermissionRequests { get; set; }

        public IList<ToolPermissionRequest> PendingPermissionRequests { get; set; }
    }
}
