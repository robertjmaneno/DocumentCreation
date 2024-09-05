using System;

namespace FolderProjectAPI.Models
{
    public class FolderDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }
        public int? ParentId { get; set; }
    }
}
