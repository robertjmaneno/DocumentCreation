using System.Collections.Generic;
using System.Threading.Tasks;
using FolderProjectAPI.Models;

namespace FolderProjectAPI.Interfaces
{
    public interface IFolderRepository
    {
        Task<IEnumerable<Folder>> GetAllFoldersAsync();
        Task<Folder> GetFolderByPathAsync(string path);
        Task<IEnumerable<Folder>> GetFoldersAsync(string path);
        Task<Folder> CreateFolderAsync(Folder folder);
        Task<Folder> UpdateFolderAsync(Folder folder);
        Task DeleteFolderAsync(int id);
        Task<Folder> GetFolderByIdAsync(int id);
    }
}
