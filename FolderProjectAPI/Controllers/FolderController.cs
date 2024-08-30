namespace FolderProjectAPI.Controllers
{
    // Controllers/FolderController.cs
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using FolderProjectAPI.Dtos;
    using FolderProjectAPI.Interfaces;
    using FolderProjectAPI.Models;
    using Microsoft.AspNetCore.Mvc;

    namespace FolderManagementSystem.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class FolderController : ControllerBase
        {
            private readonly IFolderService _folderService;
            private readonly IMapper _mapper;

            public FolderController(IFolderService folderService, IMapper mapper)
            {
                _folderService = folderService;
                _mapper = mapper;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<FolderDto>>> GetAllFolders()
            {
                var folders = await _folderService.GetAllFoldersAsync();
                return Ok(_mapper.Map<IEnumerable<FolderDto>>(folders));
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<FolderDto>> GetFolder(int id)
            {
                var folder = await _folderService.GetFolderByIdAsync(id);
                if (folder == null)
                    return NotFound();

                return Ok(_mapper.Map<FolderDto>(folder));
            }

            [HttpPost]
            public async Task<ActionResult<FolderDto>> CreateFolder(FolderDto folderDto)
            {
                var folder = _mapper.Map<Folder>(folderDto);
                var createdFolder = await _folderService.CreateFolderAsync(folder);
                return CreatedAtAction(nameof(GetFolder), new { id = createdFolder.Id }, _mapper.Map<FolderDto>(createdFolder));
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateFolder(int id, FolderDto folderDto)
            {
                if (id != folderDto.Id)
                    return BadRequest();

                var folder = _mapper.Map<Folder>(folderDto);
                var updatedFolder = await _folderService.UpdateFolderAsync(folder);
                if (updatedFolder == null)
                    return NotFound();

                return NoContent();
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteFolder(int id)
            {
                await _folderService.DeleteFolderAsync(id);
                return NoContent();
            }
        }
    }


}
