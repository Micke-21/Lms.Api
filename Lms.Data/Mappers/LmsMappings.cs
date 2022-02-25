using AutoMapper;
using Lms.Core.Dto;
using Lms.Core.Entities;

namespace Lms.Data.Mappers
{
    public class LmsMappings : Profile
    {
        public LmsMappings()
        {
            CreateMap<Course, CourseDto>();
            CreateMap<CourseForUpdateDto, Course>();

            CreateMap<Module, ModuleDto>();
            CreateMap<ModuleForUpdateDto, Module>();
            
        }
    }
}
