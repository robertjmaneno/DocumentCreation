using System.Collections.Generic;
using System.Threading.Tasks;
using FolderProjectAPI.Data;
using FolderProjectAPI.Interfaces;
using FolderProjectAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FolderProjectAPI.Repositories
{
    public class FileItemRepository : IFileItemRepository
    {
        private readonly ApplicationDbContext _context;

        public FileItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FileItem>> GetFilesByFolderIdAsync(int folderId)
        {
            return await _context.FileItems
                .Where(f => f.FolderId == folderId)
                .ToListAsync();
        }
    }
}
