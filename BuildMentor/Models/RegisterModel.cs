using BuildMentor.Database.Enums;

namespace BuildMentor.Models
{
    public class RegisterModel
    {
        public string Login { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Address { get; set; } 

        public string Password { get; set; }    

        public string ConfirmPassword { get; set; }

        public DateTime BirthDate { get; set; }

        public BuilderJobs Job { get; set; }

        public IFormFile? UploadedAvatar { get; set; }
    }
}
