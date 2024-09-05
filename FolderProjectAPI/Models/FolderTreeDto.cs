namespace FolderProjectAPI.Models
{
    public class FolderTreeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<FolderTreeDto> Children { get; set; }
    }
}
