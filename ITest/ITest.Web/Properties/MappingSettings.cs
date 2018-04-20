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

            // Without .MaxDepth() - stackoverflow exception in GetTestById() in Test
            // Service - Ask why
            this.CreateMap<TestDto, Test>(MemberList.Source).ReverseMap().MaxDepth(3);

            this.CreateMap<QuestionDto, QuestionViewModel>()
                .ForMember(q => q.Body, o => o.MapFrom(x => x.Body));

            this.CreateMap<AnswerDto, TakeTestAnswerViewModel>()
                .ForMember(a => a.Content, o => o.MapFrom(x => x.Content));

            this.CreateMap<CategoryDto, Category>(MemberList.Source)
                .ReverseMap().MaxDepth(3);

            this.CreateMap<CategoryDto, CategoryViewModel>()
                .ForMember(c => c.Name, o => o.MapFrom(x => x.Name));
        }
    }
}
