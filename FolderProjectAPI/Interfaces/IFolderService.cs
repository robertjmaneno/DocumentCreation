using FolderProjectAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FolderProjectAPI.Interfaces
{
    public interface IFolderService
    {
        Task<IEnumerable<FolderDto>> GetAllFoldersAsync();
        Task<FolderDto> GetFolderByPathAsync(string path);
        Task<IEnumerable<FolderDto>> GetFoldersAsync(string path);
        Task<FolderDto> CreateFolderAsync(CreateFolderDto createFolderDto);
        Task<FolderContentsDto> GetFolderContentsAsync(string path);
        Task<FolderDto> UpdateFolderAsync(int id, string newName);
        Task DeleteFolderAsync(int id);
    }
}