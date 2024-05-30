using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BuildMentor.Database.Entities
{
    public class ToolPermissionRequest
    {
        [Key]
        public int Id { get; set; }

        public string Comment { get; set; }

        public DateTime CreatedDate { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public User User { get; set; }

        [ForeignKey(nameof(Tool))]
        public int ToolId { get; set; }

        public Tool Tool { get; set; }

    }
}
