using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ITest.Models;
using Itest.Data.Models;

namespace ITest.Data
{
    public class ITestDbContext : IdentityDbContext<ApplicationUser>
    {
        public ITestDbContext(DbContextOptions<ITestDbContext> options)
            : base(options)
        {
        }


        public DbSet<Answer> Answers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<UserTest> UserTests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Test-To-Users
            builder.Entity<Test>()
                .HasOne(t => t.CreatedByUser)
                .WithMany(u => u.Tests)
                .HasForeignKey(t => t.CreatedByUserId);

            // UserTest
            builder.Entity<UserTest>()
                .HasOne(t => t.Test)
                .WithMany(t => t.UserTests)
                .HasForeignKey(t => t.TestId);

            builder.Entity<UserTest>()
                .HasOne(t => t.User)
                .WithMany(t => t.UserTests)
                .HasForeignKey(t => t.UserId);

            // Composite key
            builder.Entity<UserTest>()
                .HasKey(u => new { u.UserId, u.TestId });

            // Category-To-Tests
            builder.Entity<Test>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Tests)
                .HasForeignKey(t => t.CategoryId);

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

        }
    }
}
