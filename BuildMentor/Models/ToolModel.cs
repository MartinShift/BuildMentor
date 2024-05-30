using BuildMentor.Database.Enums;

namespace BuildMentor.Models
{
    public class ToolModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public IFormFile? UploadedImage { get; set; }

        public decimal Price { get; set; }
    }
}
