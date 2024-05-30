using System.ComponentModel.DataAnnotations.Schema;

namespace BuildMentor.Database.Entities
{
    public class ExternalResource
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Link { get; set; }
    }
}
