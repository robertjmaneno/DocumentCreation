using System.Collections.Generic;

namespace WebClient.Models
{
    public class FolderContentsDto
    {
        public FolderContentsDto()
        {
            Subfolders = new List<FolderDto>();
            Files = new List<FileDto>();
        }
        public int Id { get; set; }
        public string Path { get; set; }
        public List<FolderDto> Subfolders { get; set; }
        public List<FileDto> Files { get; set; }
    }
}