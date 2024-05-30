using BuildMentor.Database.Entities;

namespace BuildMentor.Models
{
    public class ToolUpdateModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public Image Image { get; set; }

        public IFormFile? UploadedImage { get; set; }
    }
}
