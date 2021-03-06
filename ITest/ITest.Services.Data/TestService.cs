﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Itest.Data.Models;
using ITest.Data.Repository;
using ITest.Data.UnitOfWork;
using ITest.DTO;
using ITest.Infrastructure.Providers.Contracts;
using ITest.Models;
using ITest.Services.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ITest.DTO.TakeTest;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace ITest.Services.Data
{
    // routing tests
    // model tests
    // return right view

    public class TestService : ITestService
    {
        private readonly IDataRepository<Test> testRepo;
        private readonly IDataRepository<Question> questionRepo;
        private readonly IDataRepository<Answer> answerRepo;
        private readonly IDataRepository<Category> categoryRepo;
        private readonly IDataSaver dataSaver;
        private readonly IMappingProvider mapper;
        private readonly IRandomProvider random;
        private readonly IShuffleProvider shuffler;
        private readonly IMemoryCache memoryCache;

        public TestService(
            IDataRepository<Test> testRepo, IDataRepository<Question> questionRepo, IDataRepository<Answer> answerRepo, IDataSaver dataSaver,
            IMappingProvider mapper, IDataRepository<Category> categoryRepo,
            IRandomProvider random, IShuffleProvider shuffler, IMemoryCache memoryCache)
        {
            this.testRepo = testRepo ?? throw new ArgumentNullException(nameof(testRepo));
            this.questionRepo = questionRepo ?? throw new ArgumentNullException(nameof(questionRepo));
            this.answerRepo = answerRepo ?? throw new ArgumentNullException(nameof(answerRepo));
            this.dataSaver = dataSaver ?? throw new ArgumentNullException(nameof(dataSaver));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.categoryRepo = categoryRepo ?? throw new ArgumentNullException(nameof(categoryRepo));
            this.random = random;
            this.shuffler = shuffler ?? throw new ArgumentNullException(nameof(shuffler));
            this.memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public TestDto GetTestById(string testId)
        {
            if (string.IsNullOrEmpty(testId))
            {
                throw new ArgumentNullException("Test Id cannot be null!");
            }

            var test = testRepo.All
                .Where(t => t.Id.ToString() == testId)
                .Include(t => t.Category)
                .FirstOrDefault();

            if (test == null)
            {
                throw new ArgumentNullException("Test not found!");
            }

            var testDto = this.mapper.MapTo<TestDto>(test);

            return testDto;
        }

        public TestDto GetRandomTest(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                throw new ArgumentNullException("Category name cannot be null!");
            }

            var allTestsFromCategory = testRepo.All
                .Where(t => t.Category.Name == categoryName &&
                            t.IsPublished)
                .ToList();

            if (allTestsFromCategory.Count == 0)
            {
                throw new ArgumentException("No created Test in this Category");
            }

            int r = this.random.Next(allTestsFromCategory.Count);
            var randomTest = allTestsFromCategory[r];

            var randomTestDto = this.mapper.MapTo<TestDto>(randomTest);

            return randomTestDto;
        }

        public TestDto GetTestQuestionsWithAnswers(string testId)
        {
            if (string.IsNullOrEmpty(testId))
            {
                throw new ArgumentNullException("Test Id cannot be null!");
            }

            var testWithQuestionsAndAnswers = this.testRepo.All
                                        .Where(t => t.Id.ToString() == testId)
                                        .Include(t => t.Category)
                                        .Include(t => t.Questions)
                                            .ThenInclude(q => q.Answers)
                                        .SingleOrDefault();

            if (testWithQuestionsAndAnswers == null)
            {
                throw new ArgumentNullException("Test not found!");
            }

            testWithQuestionsAndAnswers.Questions = testWithQuestionsAndAnswers
                                                        .Questions.Where(q => !q.IsDeleted).ToList();

            foreach (var question in testWithQuestionsAndAnswers.Questions)
            {
                question.Answers = question.Answers.Where(a => !a.IsDeleted).ToList();
            }

            if (testWithQuestionsAndAnswers == null)
            {
                throw new ArgumentNullException("Test not found!");
            }

            var testWithQuestionsAndAnswersDto = this.mapper.MapTo<TestDto>(testWithQuestionsAndAnswers);

            return testWithQuestionsAndAnswersDto;
        }

        public void ShuffleTest(TestDto testToShuffle)
        {
            if (testToShuffle == null)
            {
                throw new ArgumentNullException(nameof(testToShuffle));
            }

            var shuffledQuestions = this.shuffler.Shuffle<QuestionDto>(testToShuffle.Questions);
            testToShuffle.Questions = shuffledQuestions;

            for (int i = 0; i < testToShuffle.Questions.Count(); i++)
            {
                var shuffledAnswers = this
                    .shuffler.Shuffle<AnswerDto>(testToShuffle.Questions[i].Answers);

                testToShuffle.Questions[i].Answers = shuffledAnswers;
            }
        }

        public void CreateTest(ManageTestDto testDto)
        {
            if (testDto == null)
            {
                throw new ArgumentNullException(nameof(testDto));
            }

            var testToAdd = this.mapper.MapTo<Test>(testDto);

            var category = this.categoryRepo.All.SingleOrDefault(c => c.Name == testDto.CategoryName)
                ?? throw new ArgumentException($"Category {testDto.CategoryName} does not exists!");
            testToAdd.Category = category;

            this.testRepo.Add(testToAdd);

            this.dataSaver.SaveChanges();

            this.memoryCache.Remove("Dashboard-Tests");
        }

        private bool IsTestPassed(int testQuestionsCount, int totalCorrectQuestions)
        {
            if (testQuestionsCount <= 0)
            {
                throw new ArgumentException("Test questions count cannot be less or equal to zero");
            }

            if (totalCorrectQuestions < 0)
            {
                throw new ArgumentException("Test questions count cannot be less then zero");
            }

            var isPassed = false;
            var resultPercentage = (totalCorrectQuestions / testQuestionsCount) * 100;
            if (resultPercentage >= 80D)
            {
                isPassed = true;
            }

            // return isPassed, resultPercentange, totalQuestionsCount, totalCorrectQuestions
            return isPassed;
        }

        public bool IsTestPassed(string testId, TestRequestDto submitedTest, TestDto testWithQuestions)
        {
            if (string.IsNullOrEmpty(testId))
            {
                throw new ArgumentNullException("Test Id cannot be null!");
            }

            if (submitedTest == null)
            {
                throw new ArgumentNullException(nameof(submitedTest));
            }

            var totalCorrectQuestions = 0;

            for (int i = 0; i < testWithQuestions.Questions.Count; i++)
            {
                var submitedQuestionId = submitedTest.Questions[i].Id;
                var matchingQuestion = testWithQuestions.Questions
                        .FirstOrDefault(q => q.Id == submitedQuestionId);

                var correctAnswer = matchingQuestion.Answers
                    .Where(x => x.IsCorrect == true)
                    .FirstOrDefault();

                var selectedAnswer = submitedTest.Questions[i].Answers;

                if (selectedAnswer == null)
                {
                    continue;
                }

                if (correctAnswer.Id == selectedAnswer)
                {
                    totalCorrectQuestions++;
                }
            }

            return this.IsTestPassed(testWithQuestions.Questions.Count(), totalCorrectQuestions);
        }

        public IEnumerable<TestDashBoardDto> GetTestsDashboardInfo()
        {
            IEnumerable<TestDashBoardDto> testDtos;

            if (!this.memoryCache.TryGetValue("Dashboard-Tests", out testDtos))
            {
                var tests = this.testRepo.All;
                testDtos = this.mapper.EnumerableProjectTo<Test, TestDashBoardDto>(tests).ToList();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromHours(8));

                this.memoryCache.Set("Dashboard-Tests", testDtos, cacheEntryOptions);
            }

            return testDtos;
        }

        public ManageTestDto GetTestByNameAndCategory(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("Id cannot be null!");
            }

            var test = this.testRepo.All
                .Where(t => t.Id.ToString() == id)
                .Include(t => t.Category)
                .Include(t => t.Questions)
                .ThenInclude(q => q.Answers)
                .SingleOrDefault();

            test.Questions = test.Questions.Where(q => !q.IsDeleted).ToList();
            foreach (var question in test.Questions)
            {
                question.Answers = question.Answers.Where(a => !a.IsDeleted).ToList();
            }

            return this.mapper.MapTo<ManageTestDto>(test);
        }

        public void EditTest(ManageTestDto updatedTest)
        {
            if (updatedTest == null)
            {
                throw new ArgumentNullException(nameof(updatedTest));
            }

            var existingTest = this.testRepo.All
                .Where(t => t.Id.ToString() == updatedTest.Id)
                .Include(t => t.Questions)
                .ThenInclude(q => q.Answers)
                .SingleOrDefault();

            this.EditTest(updatedTest, existingTest);

            var category = this.categoryRepo.All.SingleOrDefault(c => c.Name == updatedTest.CategoryName)
                ?? throw new ArgumentException($"Category {updatedTest.CategoryName} does not exists!");

            existingTest.Category = category;

            this.dataSaver.SaveChanges();

            this.memoryCache.Remove("Dashboard-Tests");
        }

        private void EditTest(ManageTestDto updatedTest, Test existingTest)
        {
            existingTest.Name = updatedTest.TestName;
            existingTest.Duration = updatedTest.RequestedTime;
            existingTest.IsPublished = updatedTest.IsPublished;

            var existingQuestionsIds = existingTest.Questions.Select(eq => eq.Id.ToString()).ToHashSet();

            foreach (var updatedQuestion in updatedTest.Questions)
            {
                if (existingQuestionsIds.Contains(updatedQuestion.Id))
                {
                    //update
                    var existingQuestion = existingTest.Questions.SingleOrDefault(q => q.Id.ToString() == updatedQuestion.Id);
                    this.EditQuestion(existingQuestion, updatedQuestion);
                    existingQuestionsIds.Remove(updatedQuestion.Id);
                }
                else
                {
                    //add
                    var questiontoAdd = this.mapper.MapTo<Question>(updatedQuestion);
                    existingTest.Questions.Add(questiontoAdd);
                    this.questionRepo.Add(questiontoAdd);
                }
            }

            foreach (var questionToDeleteId in existingQuestionsIds)
            {
                //delete
                var questionToDelete = existingTest.Questions.SingleOrDefault(q => q.Id.ToString() == questionToDeleteId);
                this.DeleteQuestion(questionToDelete);
            }

            this.testRepo.Update(existingTest);
        }

        private void EditQuestion(Question existingQuestion, ManageQuestionDto updatedQuestion)
        {
            existingQuestion.Body = updatedQuestion.Body;

            var existingAnswersIds = existingQuestion.Answers.Select(ea => ea.Id.ToString()).ToHashSet();

            foreach (var updatedAnswer in updatedQuestion.Answers)
            {
                if (existingAnswersIds.Contains(updatedAnswer.Id))
                {
                    //update
                    var existingAnswer = existingQuestion.Answers.SingleOrDefault(q => q.Id.ToString() == updatedAnswer.Id);
                    this.EditAnswer(existingAnswer, updatedAnswer);
                    existingAnswersIds.Remove(updatedAnswer.Id);
                }
                else
                {
                    //add
                    var answerToAdd = this.mapper.MapTo<Answer>(updatedAnswer);
                    existingQuestion.Answers.Add(answerToAdd);
                    this.answerRepo.Add(answerToAdd);
                }
            }

            foreach (var answerToDeleteId in existingAnswersIds)
            {
                //delete
                var answerToDelete = existingQuestion.Answers.SingleOrDefault(q => q.Id.ToString() == answerToDeleteId);
                this.answerRepo.Delete(answerToDelete);
            }

            this.questionRepo.Update(existingQuestion);
        }

        private void EditAnswer(Answer existingAnswer, ManageAnswerDto updatedAnswer)
        {
            existingAnswer.Content = updatedAnswer.Content;
            existingAnswer.IsCorrect = updatedAnswer.IsCorrect;

            this.answerRepo.Update(existingAnswer);
        }

        public void PublishTest(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("Id cannot be null!");
            }

            var test = this.testRepo.All
                .Include(t => t.Category)
                .Where(t => t.Id.ToString() == id)
                .FirstOrDefault();

            if (!test.IsPublished)
            {
                test.IsPublished = true;
                test.IsDisabled = false;

                this.testRepo.Update(test);
                this.dataSaver.SaveChanges();

                this.memoryCache.Remove("Dashboard-Tests");
            }
        }

        public void DeleteTest(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("Id cannot be null!");
            }

            var test = this.testRepo.All
                .Include(t => t.Category)
                .Where(t => t.Id.ToString() == id)
                .Include(t => t.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefault();

            if (!test.IsDeleted)
            {
                this.DeleteTest(test);

                this.dataSaver.SaveChanges();

                this.memoryCache.Remove("Dashboard-Tests");
            }
        }

        private void DeleteTest(Test test)
        {
            this.testRepo.Delete(test);

            foreach (var question in test.Questions)
            {
                this.DeleteQuestion(question);
            }
        }

        private void DeleteQuestion(Question question)
        {
            this.questionRepo.Delete(question);

            foreach (var answer in question.Answers)
            {
                this.answerRepo.Delete(answer);
            }
        }

        public void DisableTest(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("Id cannot be null!");
            }

            var test = this.testRepo.All
                .Include(t => t.Category)
                .Where(t => t.Id.ToString() == id)
                .FirstOrDefault();

            if (test.IsPublished)
            {
                test.IsPublished = false;
                test.IsDisabled = true;

                this.testRepo.Update(test);
                this.dataSaver.SaveChanges();

                this.memoryCache.Remove("Dashboard-Tests");
            }
        }
    }
}
