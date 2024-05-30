using BuildMentor.Database.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BuildMentor.Database.Entities
{
    public class UserNotification
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public User User { get; set; }

        [ForeignKey(nameof(Admin))]
        public int AdminId { get; set; }

        public User Admin { get; set; }

        public NotificationType Type { get; set; }

        public string Comment { get; set; }
    }
}
