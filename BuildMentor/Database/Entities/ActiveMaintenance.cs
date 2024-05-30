using System.ComponentModel.DataAnnotations.Schema;

namespace BuildMentor.Database.Entities
{
    public class ActiveMaintenance
    {
        public int Id { get; set; }

        [ForeignKey(nameof(UserTool))]
        public int UserToolId { get; set; }

        public UserTool UserTool { get; set; }

        public string Comment { get; set; }

        public decimal Price { get; set; }
    }
}
