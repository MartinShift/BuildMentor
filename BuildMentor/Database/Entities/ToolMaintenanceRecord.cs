using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildMentor.Database.Entities
{
    public class ToolMaintenanceRecord
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public decimal Price { get; set; }

        [ForeignKey(nameof(Document))]
        public int DocumentId { get; set; }

        public Image Document { get; set; }

        [ForeignKey(nameof(Tool))]
        public int ToolId { get; set; }

        public UserTool Tool { get; set; }

        
    }
}
