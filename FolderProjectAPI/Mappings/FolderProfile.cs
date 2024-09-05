using AutoMapper;
using FolderProjectAPI.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FolderProjectAPI.Mappings
{
    public class FolderProfile : Profile
    {
        public FolderProfile()
        {
            CreateMap<Folder, FolderDto>();
            CreateMap<FileItem, FileDto>();
            CreateMap<CreateFolderDto, Folder>();
            CreateMap<CreateFileDto, FileItem>();
            CreateMap<Folder, FolderDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));


        }
    }
}
