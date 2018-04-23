using AutoMapper;
using Itest.Data.Models;
using ITest.DTO;
using ITest.Web.Areas.Admin.Models.ManageViewModels;
using ITest.Web.Areas.User.Models;

namespace ITest.Web.Properties
{
    public class MappingSettings : Profile
    {
        public MappingSettings()
        {
            //Test:
            //From ViewModel to Dto
            this.CreateMap<CreateTestViewModel, CreateTestDto>(MemberList.Source)
                .ForMember(t => t.Questions, o => o.MapFrom(t => t.Questions));

            this.CreateMap<CreateQuestionViewModel, CreateQuestionDto>(MemberList.Source)
                .ForMember(q => q.Answers, o => o.MapFrom(q => q.Answers));

            this.CreateMap<CreateAnswerViewModel, CreateAnswerDto>(MemberList.Source)
                .ForMember(a => a.Content, o => o.MapFrom(a => a.Content))
                .ForMember(a => a.IsCorrect, o => o.MapFrom(a => a.IsCorrect));

            //From Dto to DataModel
            this.CreateMap<CreateTestDto, Test>(MemberList.Source)
                .ForMember(t => t.Name, o => o.MapFrom(t => t.TestName))
                .ForMember(t => t.Duration, o => o.MapFrom(t => t.RequestedTime))
                .ForMember(t => t.Questions, o => o.MapFrom(t => t.Questions));

            this.CreateMap<CreateQuestionDto, Question>(MemberList.Source)
                .ForMember(q => q.Answers, o => o.MapFrom(q => q.Answers));

            this.CreateMap<CreateAnswerDto, Answer>(MemberList.Source);


            this.CreateMap<TestDto, Areas.User.Models.TestViewModel>()
                .ForMember(t => t.Name, o => o.MapFrom(x => x.Name))
                .ForMember(t => t.CategoryName, o => o.MapFrom(x => x.Category.Name))
                //.ForMember(t => t.Id, o => o.MapFrom(x => x.Id))
                .ForMember(t => t.Questions, o => o.MapFrom(x => x.Questions))
                .MaxDepth(3);

            // Without .MaxDepth() - stackoverflow exception in GetTestById() in Test
            // Service - Ask why
            this.CreateMap<Test, TestDto>(MemberList.Source)
                .ForMember(q => q.Questions, o => o.MapFrom(x => x.Questions))
                .MaxDepth(3)
                .ReverseMap();

            this.CreateMap<Question, QuestionDto>()
                .ForMember(q => q.Answers, o => o.MapFrom(x => x.Answers));

            this.CreateMap<Answer, AnswerDto>();

            this.CreateMap<QuestionDto, QuestionViewModel>()
                .ForMember(q => q.Answers, o => o.MapFrom(x => x.Answers))
                .MaxDepth(3);
            //.ForMember(q => q.Body, o => o.MapFrom(x => x.Body))
            //.ForMember(q => q.Id, o => o.MapFrom(x => x.Id));

            this.CreateMap<AnswerDto, TakeTestAnswerViewModel>();

            this.CreateMap<CategoryDto, Category>(MemberList.Source)
                .ReverseMap().MaxDepth(3);

            this.CreateMap<CategoryDto, CategoryViewModel>()
                .ForMember(c => c.Name, o => o.MapFrom(x => x.Name));
        }
    }
}
