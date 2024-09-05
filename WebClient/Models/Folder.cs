using System;
namespace WebClient.Models
{
    public class Folder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }
        public int? ParentId { get; set; } // Nullable for root folders
        public virtual Folder Parent { get; set; }
        public virtual ICollection<Folder> Subfolders { get; set; } = new HashSet<Folder>();
        public virtual ICollection<FileItem> Files { get; set; } = new HashSet<FileItem>();
    }
}
