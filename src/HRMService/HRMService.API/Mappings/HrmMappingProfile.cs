using HRMService.API.Models;
using HRMService.Domain.Entities;
using AutoMapper;

namespace HRMService.API.Mappings
{
    public class HrmMappingProfile : Profile
    {
        public HrmMappingProfile()
        {

            CreateMap<HrmEmployee, HrmEmployeeDto>();

            CreateMap<HrmEmployeeDto, HrmEmployee>();

            CreateMap<HrmDepartment, HrmDepartmentDto>();

            CreateMap<HrmDepartmentDto, HrmDepartment>();
        }
    }
}
