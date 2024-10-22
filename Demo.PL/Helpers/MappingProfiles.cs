using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;

namespace Demo.PL.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
            CreateMap<DepartmentViewModel, Department>().ReverseMap();

            //مختلف Propertiesو اسماء ال  EmployeeViewModel ل Emp في حالة اني جيت احول من ال 
            
            //CreateMap<EmployeeViewModel, Employee>()
            //    .ForMember(D => D.Name, O => O.EmpName );
        }
    }
}
