namespace FolderProjectAPI.Models
{
    public class CreateFolderDto
    {
        public string Name { get; set; }
        public int? ParentId { get; set; }  // null for root folders, folder ID for subfolders
    }
}
