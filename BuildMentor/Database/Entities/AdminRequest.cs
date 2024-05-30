using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildMentor.Database.Entities
{
    public class AdminRequest
    {
        [Key]
        public int Id { get; set; }

        public string Message { get; set; }

        [ForeignKey(nameof(Sender))]
        public int SenderId { get; set; }

        public User Sender { get; set; }
    }
}
