using AutoMapper;
using Itest.Data.Models;
using ITest.DTO;
using ITest.Web.Areas.User.Models;

namespace ITest.Web.Properties
{
    public class MappingSettings : Profile
    {
        public MappingSettings()
        {
            this.CreateMap<TestDto, TestViewModel>()
                .ForMember(t => t.Name, o => o.MapFrom(x => x.Name))
                .ForMember(t => t.CategoryName, o => o.MapFrom(x => x.Category.Name))
                .ForMember(t => t.Id, o => o.MapFrom(x => x.Id));
            this.CreateMap<TestDto, Test>(MemberList.Source);
        }
    }
}
