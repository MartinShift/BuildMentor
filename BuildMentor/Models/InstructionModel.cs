using BuildMentor.Database.Entities;

namespace BuildMentor.Models
{
    public class InstructionModel
    {
        public int? Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public List<ExternalResource> ExternalResources {  get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        public int ToolId { get; set; }
    }
}
