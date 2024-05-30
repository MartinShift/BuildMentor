using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildMentor.Database.Entities
{
    public class ToolPermission
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public User User { get; set; }

        [ForeignKey(nameof(Admin))]
        public int AdminId { get; set; }

        public User Admin { get; set; }

        [ForeignKey(nameof(Tool))]
        public int ToolId { get; set; }

        public Tool Tool { get; set; }
    }
}
