using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildMentor.Database.Entities
{
    public class Instruction
    {
        [Key]
        public int Id { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }

        public DateTime CreatedDate { get; set; }

        public IList<ExternalResource> ExternalResources { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        [ForeignKey(nameof(Tool))]
        public int ToolId { get; set; }
        
        public Tool Tool { get; set; }
    }
}
