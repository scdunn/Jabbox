using AutoMapper;
using Jabbox.API.Models;
using Jabbox.Data.Models;

namespace Jabbox.API.DTOs
{
    /// <summary>
    /// Auto map mapping profile for DTO/Data Models
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PostDTO, Post>().ReverseMap();
        }
    }
}
