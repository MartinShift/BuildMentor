using BuildMentor.Database.Enums;

namespace BuildMentor.Models
{
    public class FilterModel
    {
        public string SearchText { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public Condition? Condition { get; set; }
    }

}
