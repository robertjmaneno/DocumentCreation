using System.ComponentModel.DataAnnotations;

namespace WebClient.Models
{
    public class FileItemViewModel
    {
        public string Name { get; set; }

        [Required]
        public string Path { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }
    }
}
