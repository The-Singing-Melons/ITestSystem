using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ITest.Models;
using Itest.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace ITest.Data
{
    public class ITestDbContext : IdentityDbContext<ApplicationUser>
    {
        public ITestDbContext(DbContextOptions<ITestDbContext> options)
            : base(options)
        {
            this.Seed().Wait();
        }

        public DbSet<Answer> Answers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<UserTest> UserTests { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Users-To-Answers
            builder.Entity<UserAnswer>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UserAnswers)
                .HasForeignKey(ua => ua.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserAnswer>()
                .HasOne(ut => ut.Answer)
                .WithMany(a => a.UserAnswers)
                .HasForeignKey(ua => ua.AnswerId);

            builder.Entity<UserAnswer>()
                .HasKey(ua => new { ua.UserId, ua.AnswerId });


            // Users-To-Tests
            builder.Entity<UserTest>()
                .HasOne(ut => ut.Test)
                .WithMany(t => t.UserTests)
                .HasForeignKey(t => t.TestId);

            builder.Entity<UserTest>()
                .HasOne(t => t.User)
                .WithMany(t => t.UserTests)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserTest>()
                .HasKey(ut => new { ut.UserId, ut.TestId });


            // Test-To-Users
            builder.Entity<Test>()
                .HasOne(t => t.CreatedByUser)
                .WithMany(u => u.Tests)
                .HasForeignKey(t => t.CreatedByUserId);


            // Category-To-Tests
            builder.Entity<Test>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Tests)
                .HasForeignKey(t => t.CategoryId);

            builder.Entity<Test>()
                .HasIndex(t => t.Name)
                .IsUnique(true);

            // Test-To-Questions
            builder.Entity<Question>()
                .HasOne(q => q.Test)
                .WithMany(t => t.Questions)
                .HasForeignKey(q => q.TestId);

            // Question-To-Answers
            builder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId);


            builder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique(true);


            base.OnModelCreating(builder);
        }

        private async Task Seed()
        {
            this.Database.EnsureCreated();

            if (!this.Roles.Any(r => r.Name == "Administrator"))
            {
                var adminRole = new IdentityRole("Administrator");
                this.Roles.Add(adminRole);
            }

            if (!this.Categories.Any())
            {
                var categoriesToAdd = new List<Category>()
            {
                new Category()
                {
                    Name = "Java"
                },

                new Category()
                {
                    Name = ".NET"
                },

                new Category()
                {
                    Name = "Javascript"
                },

                new Category()
                {
                    Name = "SQL"
                }
            };

                categoriesToAdd
                    .ForEach(c => this.Categories.Add(c));

            }

            await this.SaveChangesAsync();
        }
    }
}
