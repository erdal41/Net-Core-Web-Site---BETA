using MySite.Shared.Entities.Abstract;

namespace MySite.Entities.Concrete
{
    public class Upload : EntityUserBase, IEntity
    {
        public string FileName { get; set; }
        public string AltText { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string FileUrl { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
    }
}