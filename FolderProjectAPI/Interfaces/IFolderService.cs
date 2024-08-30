using System.Collections.Generic;
using System.Threading.Tasks;
using FolderProjectAPI.Models;

namespace FolderProjectAPI.Interfaces
{
    public interface IFolderService
    {
        Task<IEnumerable<Folder>> GetAllFoldersAsync();
        Task<Folder> GetFolderByIdAsync(int id);
        Task<Folder> CreateFolderAsync(Folder folder);
        Task<Folder> UpdateFolderAsync(Folder folder);
        Task DeleteFolderAsync(int id);
    }
}