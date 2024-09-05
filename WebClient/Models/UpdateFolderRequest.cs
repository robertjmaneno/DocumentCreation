using System.ComponentModel.DataAnnotations;

namespace WebClient.Models
{
    public class UpdateFolderRequest
    {
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Name { get; set; }
    }
}
