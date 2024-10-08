﻿using System;
using System.Collections.Generic;

namespace FolderProjectAPI.Models
{
    public class Folder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }
        public int? ParentId { get; set; }
        public virtual Folder Parent { get; set; }
        public virtual ICollection<Folder> Subfolders { get; set; }
        public virtual ICollection<FileItem> Files { get; set; }
    }
}
