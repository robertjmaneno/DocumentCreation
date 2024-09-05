using System.ComponentModel.DataAnnotations;

namespace FolderProjectAPI.Models
{
    public class CreateFileDto
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public string ContentType { get; set; }

        public long Size { get; set; }

        [Required]
        public string FolderPath { get; set; }
    }
}
