namespace MySite.Entities.Dtos
{
    public class FileDeleteDto
    {
        public string FileFullName { get; set; }
        public string ContentType { get; set; }
        public string Extension { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
    }
}