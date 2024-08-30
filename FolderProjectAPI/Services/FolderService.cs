using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FolderProjectAPI.Data;
using FolderProjectAPI.Interfaces;
using FolderProjectAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FolderManagementSystem.Services
{
    public class FolderService : IFolderService
    {
        private readonly ApplicationDbContext _context;

        public FolderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Folder>> GetAllFoldersAsync()
        {
            return await _context.Folders.ToListAsync();
        }

        public async Task<Folder> GetFolderByIdAsync(int id)
        {
            return await _context.Folders.FindAsync(id);
        }

        public async Task<Folder> CreateFolderAsync(Folder folder)
        {
            folder.CreatedAt = DateTime.UtcNow;
            folder.UpdatedAt = DateTime.UtcNow;
            _context.Folders.Add(folder);
            await _context.SaveChangesAsync();
            return folder;
        }

        public async Task<Folder> UpdateFolderAsync(Folder folder)
        {
            var existingFolder = await _context.Folders.FindAsync(folder.Id);
            if (existingFolder == null)
                return null;

            existingFolder.Name = folder.Name;
            existingFolder.ParentId = folder.ParentId;
            existingFolder.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingFolder;
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
    }
}
