using BuildMentor.Database.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildMentor.Database.Entities
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        [ForeignKey(nameof(UserTool))]
        public int UserToolId { get; set; }

        public UserTool UserTool { get; set; }

        [ForeignKey(nameof(Admin))]
        public int AdminId { get; set; }

        public User Admin { get; set; }

        public NotificationType Type { get; set; }

        public string Comment { get; set; }
    }
}
