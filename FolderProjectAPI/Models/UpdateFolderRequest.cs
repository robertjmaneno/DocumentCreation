using System.ComponentModel.DataAnnotations;

namespace FolderProjectAPI.Models
{
    public class UpdateFolderRequest
    {
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Name { get; set; }
    }
}
