using System;
using AutoMapper;
using Itest.Data.Models;
using ITest.DTO;
using ITest.DTO.TakeTest;
using ITest.DTO.UserHome.Index;
using ITest.Web.Areas.Admin.Models.ManageViewModels;
using ITest.Web.Areas.User.Models;

namespace ITest.Web.Properties
{
    public class MappingSettings : Profile
    {
        public MappingSettings()
        {
            this.ViewModelsAndDtosMappings();
            this.DtosAndDataModelsMappings();
        }

        private void ViewModelsAndDtosMappings()
        {
            //Test:
            //From ViewModel to Dto
            this.CreateMap<ManageTestViewModel, ManageTestDto>()
                .ForMember(t => t.Questions, o => o.MapFrom(t => t.Questions))
                .ReverseMap();

            this.CreateMap<ManageQuestionViewModel, ManageQuestionDto>()
                .ForMember(q => q.Answers, o => o.MapFrom(q => q.Answers))
                .ReverseMap();

            this.CreateMap<ManageAnswerViewModel, ManageAnswerDto>()
                .ReverseMap();


            //From Dto to ViewModel
            this.CreateMap<TestDashBoardDto, CreatedTestViewModel>(MemberList.Destination);

            this.CreateMap<TestDto, TestViewModel>()
                .ForMember(t => t.Name, o => o.MapFrom(x => x.Name))
                .ForMember(t => t.CategoryName, o => o.MapFrom(x => x.Category.Name))
                //.ForMember(t => t.Id, o => o.MapFrom(x => x.Id))
                .ForMember(t => t.Questions, o => o.MapFrom(x => x.Questions))
                .MaxDepth(3)
                .ReverseMap();


            // TakeTestDto to TakeTestViewModels
            this.CreateMap<TestRequestDto, TestRequestViewModel>()
                .ForMember(x => x.Questions, o => o.MapFrom(x => x.Questions));

            this.CreateMap<QuestionResponseDto, QuestionResponseModel>();

            // CategoryIndexDto to CategoryView 
            this.CreateMap<CategoryIndexDto, CategoryViewModel>();


            this.CreateMap<QuestionDto, QuestionViewModel>()
                .ForMember(q => q.Answers, o => o.MapFrom(x => x.Answers))
                .MaxDepth(3)
                .ReverseMap();
            //.ForMember(q => q.Body, o => o.MapFrom(x => x.Body))
            //.ForMember(q => q.Id, o => o.MapFrom(x => x.Id));

            this.CreateMap<AnswerDto, AnswerViewModel>()
                .ReverseMap();


            this.CreateMap<CategoryDto, CategoryViewModel>()
                .ForMember(c => c.Name, o => o.MapFrom(x => x.Name));


            this.CreateMap<UserTestResultDto, TestResultViewModel>(MemberList.Destination);


            this.CreateMap<UserAnswerDto, TestScoreUserDetailsViewModel>(
                MemberList.Destination)
                .ForMember(vm => vm.UserName, o => o.MapFrom(dto =>
                    dto.User.UserName))
                .ForMember(vm => vm.TestName, o => o.MapFrom(dto =>
                    dto.Answer.Question.Test.Name))
                .ForMember(vm => vm.TestCategory, o => o.MapFrom(dto =>
                    dto.Answer.Question.Test.Category.Name));

            this.CreateMap<UserAnswerDto, TestScoreAnswerForTestViewModel>
                (MemberList.Destination)
                 .ForMember(vm => vm.AnswerContent, o => o.MapFrom(dto =>
                    dto.Answer.Content))
                .ForMember(vm => vm.QuestionContent, o => o.MapFrom(dto =>
                    dto.Answer.Question.Body))
                .ForMember(vm => vm.Id, o => o.MapFrom(dto => dto.AnswerId.ToString()))
                .ForMember(vm => vm.IsCorrect, o => o.MapFrom(dto => dto.Answer.IsCorrect));
        }

        private void DtosAndDataModelsMappings()
        {
            //From Dto to DataModel
            this.CreateMap<ManageTestDto, Test>()
                .ForMember(t => t.Name, o => o.MapFrom(t => t.TestName))
                .ForMember(t => t.Duration, o => o.MapFrom(t => t.RequestedTime))
                .ForMember(t => t.Questions, o => o.MapFrom(t => t.Questions))
                .ReverseMap()
                .ForMember(t => t.Id, o => o.MapFrom(t => t.Id.ToString()))
                .ForMember(t => t.CategoryName, o => o.MapFrom(t => t.Category.Name));

            this.CreateMap<ManageQuestionDto, Question>()
                .ForMember(q => q.Answers, o => o.MapFrom(q => q.Answers))
                .ReverseMap()
                .ForMember(q => q.Id, o => o.MapFrom(q => q.Id.ToString()));

            this.CreateMap<ManageAnswerDto, Answer>()
                .ReverseMap()
                .ForMember(a => a.Id, o => o.MapFrom(a => a.Id.ToString()));


            //From Test to Dto
            this.CreateMap<Test, TestDashBoardDto>()
                .ForMember(t => t.CategoryName, o => o.MapFrom(t => t.Category.Name))
                .ForMember(t => t.TestName, o => o.MapFrom(t => t.Name));


            // Without .MaxDepth() - stackoverflow exception in GetTestById() in Test
            // Service - Ask why
            this.CreateMap<Test, TestDto>(MemberList.Source)
              .ForMember(q => q.Questions, o => o.MapFrom(x => x.Questions))
              .MaxDepth(3)
              .ReverseMap();

            this.CreateMap<Question, QuestionDto>()
                .ForMember(q => q.Answers, o => o.MapFrom(x => x.Answers));

            this.CreateMap<Answer, AnswerDto>();


            this.CreateMap<CategoryDto, Category>(MemberList.Source)
                .ReverseMap().MaxDepth(3);


            this.CreateMap<UserTest, UserTestResultDto>(MemberList.Destination)
                .ForMember(ut => ut.TestName, o => o.MapFrom(ut => ut.Test.Name))
                .ForMember(ut => ut.UserName, o => o.MapFrom(ut => ut.User.UserName))
                .ForMember(ut => ut.CategoryName, o => o.MapFrom(ut => ut.Test.Category.Name))
                .ForMember(ut => ut.RequestedTime, o => o.MapFrom(ut => ut.Test.Duration));


            this.CreateMap<UserTestDto, UserTest>().ReverseMap();


            this.CreateMap<UserAnswer, UserAnswerDto>(MemberList.Source)
                .ReverseMap();

        }
    }
}
