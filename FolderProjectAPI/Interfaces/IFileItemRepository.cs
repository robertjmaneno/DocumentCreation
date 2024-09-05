using System.Collections.Generic;
using System.Threading.Tasks;
using FolderProjectAPI.Models;

namespace FolderProjectAPI.Interfaces
{
    public interface IFileItemRepository
    {
        Task<IEnumerable<FileItem>> GetFilesByFolderIdAsync(int folderId);
    }
}
