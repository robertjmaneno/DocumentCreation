using AutoMapper;
using FolderProjectAPI.Controllers;
using FolderProjectAPI.Interfaces;
using FolderProjectAPI.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FolderProjectAPI.Services
{
    public class FolderService : IFolderService
    {
        private readonly IFolderRepository _folderRepository;
        private readonly IFileItemRepository _fileItemRepository;
        private readonly IMapper _mapper;
        private readonly string _basePath;
        private readonly ILogger<FolderService> _logger;

        public FolderService(IFolderRepository folderRepository,
            IConfiguration configuration, 
            IFileItemRepository fileItemRepository, 
            IMapper mapper,
            ILogger<FolderService> logger)
        {
            _folderRepository = folderRepository;
            _fileItemRepository = fileItemRepository;
            _basePath = configuration.GetValue<string>("FileServerSettings:BasePath");
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<FolderDto>> GetAllFoldersAsync()
        {
            var folders = await _folderRepository.GetAllFoldersAsync();
            return _mapper.Map<IEnumerable<FolderDto>>(folders);
        }

        public async Task<FolderDto> GetFolderByPathAsync(string path)
        {
            var folder = await _folderRepository.GetFolderByPathAsync(path);
            return _mapper.Map<FolderDto>(folder);
        }

        public async Task<IEnumerable<FolderDto>> GetFoldersAsync(string path)
        {
            var folders = await _folderRepository.GetFoldersAsync(path);
            return _mapper.Map<IEnumerable<FolderDto>>(folders);
        }


        public async Task<FolderDto> CreateFolderAsync(CreateFolderDto createFolderDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(createFolderDto.Name))
                {
                    throw new ArgumentException("Folder name cannot be empty");
                }

                string folderPath;
                Folder parentFolder = null;

                if (createFolderDto.ParentId == null || createFolderDto.ParentId == 0)
                {
                    // This is a root folder
                    folderPath = Path.Combine(_basePath, createFolderDto.Name);
                }
                else
                {
                    // This is a subfolder
                    parentFolder = await _folderRepository.GetFolderByIdAsync(createFolderDto.ParentId.Value);
                    if (parentFolder == null)
                    {
                        throw new ArgumentException($"Parent folder not found with ID: {createFolderDto.ParentId}");
                    }
                    folderPath = Path.Combine(parentFolder.Path, createFolderDto.Name);
                }

                // Ensure the folder doesn't already exist
                if (Directory.Exists(folderPath))
                {
                    throw new ArgumentException($"A folder with the name '{createFolderDto.Name}' already exists in this location.");
                }

                // Create the actual directory on the file system
                Directory.CreateDirectory(folderPath);

                var folder = new Folder
                {
                    Name = createFolderDto.Name,
                    Parent = parentFolder,
                    Path = folderPath, // Store the full path
                    CreatedAt = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow
                };

                await _folderRepository.CreateFolderAsync(folder);
                return _mapper.Map<FolderDto>(folder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating folder: {Message}", ex.Message);
                throw;
            }
        }

        private string GenerateFolderPath(string name, Folder parentFolder)
        {
            if (parentFolder == null)
            {
                return Path.Combine(_basePath, name); // Path for root folders
            }
            return Path.Combine(parentFolder.Path, name); // Path for subfolders
        }
        public async Task<FolderContentsDto> GetFolderContentsAsync(string path)
        {
            try
            {
                // Ensure the path is within the allowed base path
                string fullPath = Path.GetFullPath(Path.Combine(_basePath, path ?? ""));
                if (!fullPath.StartsWith(_basePath, StringComparison.OrdinalIgnoreCase))
                {
                    throw new ArgumentException("Invalid path");
                }

                if (!Directory.Exists(fullPath))
                {
                    return null;
                }

                // Retrieve the folder information from the repository by path
                var folder = await _folderRepository.GetFolderByPathAsync(fullPath);
                if (folder == null)
                {
                    // If folder does not exist in the database, create a new one in memory
                    folder = new Folder { Path = fullPath, Name = Path.GetFileName(fullPath) };
                }

                // Fetch subfolders and retrieve their IDs from the repository as well
                var subfolders = Directory.GetDirectories(fullPath)
                    .Select(async d =>
                    {
                        var subfolderFullPath = d;
                        var subfolder = await _folderRepository.GetFolderByPathAsync(subfolderFullPath);
                        return new FolderDto
                        {
                            Id = subfolder.Id, // If subfolder exist
                            Name = Path.GetFileName(d),
                            Path = d.Substring(_basePath.Length).TrimStart(Path.DirectorySeparatorChar).Replace('\\', '/')
                        };
                    }).Select(t => t.Result).ToList(); // Await the tasks


                var files = Directory.GetFiles(fullPath)
                    .Select(f => new FileItemDto
                    {
                        Name = Path.GetFileName(f),
                        Path = f.Substring(_basePath.Length).TrimStart(Path.DirectorySeparatorChar).Replace('\\', '/'),
                        Size = new FileInfo(f).Length
                    }).ToList();


                return new FolderContentsDto
                {
                    Id = folder.Id, 
                    Path = path,
                    Subfolders = subfolders,
                    Files = files
                };
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error getting folder contents", ex);
            }
        }

        public async Task<FolderDto> UpdateFolderAsync(int id, string newName)
        {
            try
            {
                var folder = await _folderRepository.GetFolderByIdAsync(id);
                if (folder == null)
                    return null;

                var oldPath = folder.Path;
                var newPath = Path.Combine(Path.GetDirectoryName(oldPath), newName);

                // Rename the actual directory on the file system
                Directory.Move(oldPath, newPath);

                folder.Name = newName;
                folder.Path = newPath.Replace('\\', '/');
                folder.LastModified = DateTime.UtcNow;

                var updatedFolder = await _folderRepository.UpdateFolderAsync(folder);
                return _mapper.Map<FolderDto>(updatedFolder);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error updating folder", ex);
            }
        }

        public async Task DeleteFolderAsync(int id)
        {
            try
            {
                var folder = await _folderRepository.GetFolderByIdAsync(id);
                if (folder != null)
                {
                    // Delete the actual directory on the file system
                    if (Directory.Exists(folder.Path))
                    {
                        Directory.Delete(folder.Path, true);
                    }

                    await _folderRepository.DeleteFolderAsync(id);
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error deleting folder", ex);
            }
        }
    }
}