using Microsoft.AspNetCore.Mvc;
using FolderProjectAPI.Interfaces;
using FolderProjectAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FolderProjectAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoldersController : ControllerBase
    {
        private readonly IFolderService _folderService;
        private readonly ILogger<FoldersController> _logger;

        public FoldersController(IFolderService folderService, ILogger<FoldersController> logger)
        {
            _folderService = folderService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FolderDto>>> GetFolders()
        {
            try
            {
                var folders = await _folderService.GetAllFoldersAsync();
                return Ok(folders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching folders");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("contents")]
        public async Task<ActionResult<FolderContentsDto>> GetFolderContents([FromQuery] string path)
        {
            try
            {
                var contents = await _folderService.GetFolderContentsAsync(path);
                if (contents == null)
                    return NotFound(new { success = false, message = "Folder not found" });
                return Ok(new { success = true, folder = contents });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid path requested: {Path}", path);
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching folder contents for path: {Path}", path);
                return StatusCode(500, new { success = false, message = "An error occurred while fetching folder contents." });
            }
        }
        [HttpPost]
        public async Task<ActionResult<FolderDto>> CreateFolder([FromBody] CreateFolderDto createFolderDto)
        {
            if (createFolderDto == null)
                return BadRequest(new { success = false, message = "Folder data is null" });

            try
            {
                var folder = await _folderService.CreateFolderAsync(createFolderDto);
                return Ok(new { success = true, folder = folder });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument when creating folder: {Message}", ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating folder");
                return StatusCode(500, new { success = false, message = "An error occurred while creating the folder." });
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<FolderDto>> UpdateFolder(int id, [FromBody] string newName)
        {
            try
            {
                var folder = await _folderService.UpdateFolderAsync(id, newName);
                if (folder == null)
                    return NotFound();

                return Ok(folder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating folder with ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFolder(int id)
        {
            try
            {
                await _folderService.DeleteFolderAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting folder with ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
