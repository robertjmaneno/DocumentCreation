using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FolderProjectAPI.Data;
using FolderProjectAPI.Interfaces;
using FolderProjectAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FolderProjectAPI.Repositories
{
    public class FolderRepository : IFolderRepository
    {
        private readonly ApplicationDbContext _context;
        public FolderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Folder>> GetAllFoldersAsync()
        {
            return await _context.Folders
                .Include(f => f.Subfolders)
                .Include(f => f.Files)
                .ToListAsync();
        }

        public async Task<Folder> GetFolderByPathAsync(string path)
        {
            return await _context.Folders
                .Include(f => f.Subfolders)
                .Include(f => f.Files)
                .FirstOrDefaultAsync(f => f.Path == path);
        }

        public async Task<IEnumerable<Folder>> GetFoldersAsync(string path)
        {
            var folder = await _context.Folders.FirstOrDefaultAsync(f => f.Path == path);
            return folder?.Subfolders ?? new List<Folder>();
        }

        public async Task<Folder> CreateFolderAsync(Folder folder)
        {
            _context.Folders.Add(folder);
            await _context.SaveChangesAsync();
            return folder;
        }

        public async Task<Folder> UpdateFolderAsync(Folder folder)
        {
            _context.Folders.Update(folder);
            await _context.SaveChangesAsync();
            return folder;
        }

        public async Task DeleteFolderAsync(int id)
        {
            var folder = await _context.Folders.FindAsync(id);
            if (folder != null)
            {
                _context.Folders.Remove(folder);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Folder> GetFolderByIdAsync(int id)
        {
            return await _context.Folders
                .Include(f => f.Subfolders)
                .Include(f => f.Files)
                .FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}