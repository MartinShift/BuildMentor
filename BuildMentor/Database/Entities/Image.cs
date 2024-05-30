namespace BuildMentor.Database.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public string Filename { get; set; }
        public string RootDirectory { get; set; }

        public string Url()
        {
            return Filename;
        }

        public string Path()
        {
            return "wwwroot/" + RootDirectory + "/" + Filename;
        }

        public string RelativePath()
        {
            return RootDirectory + "/" + Filename;
        }
    }
}
