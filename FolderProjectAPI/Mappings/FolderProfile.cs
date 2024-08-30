using AutoMapper;
using FolderProjectAPI.Dtos;
using FolderProjectAPI.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FolderProjectAPI.Mappings
{
    public class FolderProfile : Profile
    {
        public FolderProfile()
        {
            CreateMap<Folder, FolderDto>();
            CreateMap<FolderDto, Folder>();
        }
    }
}
