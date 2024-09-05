using System.Collections.Generic;

namespace FolderProjectAPI.Models
{
    public class FolderContentsDto
    {


        public int Id { get; set; }
        public string Path { get; set; }  
        public List<FolderDto> Subfolders { get; set; } = new List<FolderDto>();
        public List<FileItemDto> Files { get; set; } = new List<FileItemDto>();
    }
}
