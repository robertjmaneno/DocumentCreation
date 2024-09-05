using System;

namespace FolderProjectAPI.Models
{
    public class FileItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }
        public int FolderId { get; set; }
        public virtual Folder Folder { get; set; }
    }
}
