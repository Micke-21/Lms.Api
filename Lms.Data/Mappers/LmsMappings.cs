using AutoMapper;
using Lms.Core.Dto;
using Lms.Core.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace Lms.Data.Mappers
{
    public class LmsMappings : Profile
    {
        public LmsMappings()
        {
            CreateMap<Course, CourseDto>();
            CreateMap<CourseForUpdateDto, Course>();
            CreateMap<Course, CourseForUpdateDto>();

            CreateMap<JsonPatchDocument<CourseForUpdateDto>, JsonPatchDocument<Course>>();
            CreateMap<Operation<CourseForUpdateDto>, Operation<Course>>();


            CreateMap<Module, ModuleDto>();
            CreateMap<ModuleForUpdateDto, Module>();

            CreateMap<JsonPatchDocument<ModuleForUpdateDto>, JsonPatchDocument<Module>>();
            CreateMap<Operation<ModuleForUpdateDto>, Operation<Module>>();
        }
    }
}
