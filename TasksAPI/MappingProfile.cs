using AutoMapper;
using TasksAPI.Entities;
using TasksAPI.Models;

namespace TasksAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDto, User>();
            CreateMap<CreateGroupDto, Group>();
            CreateMap<Group, GroupDto>();
        }
    }
}
