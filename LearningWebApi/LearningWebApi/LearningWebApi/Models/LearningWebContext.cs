using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningWebApi.Models
{
    public class LearningWebContext: DbContext
    {
        public LearningWebContext(DbContextOptions<LearningWebContext> options) : base(options)
        {

        }


        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }



        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            //foreach (var relationship in modelbuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            //{
            //    relationship.DeleteBehavior = DeleteBehavior.Restrict;
            //}

            //modelbuilder.Entity<ProjectResponse>().HasOne(t => t.Teacher)
            //                            .WithMany(t => t.Bids)
            //                            .OnDelete(DeleteBehavior.NoAction);


            //modelbuilder.Entity<QueryResponse>().HasOne(q=>q.Teacher)
            //                           .WithMany(q=>q.QueryResponses)
            //                           .OnDelete(DeleteBehavior.NoAction);

            //modelbuilder.Entity<Enrollment>().HasOne(e => e.Student)
            //                           .WithMany(e => e.Enrollments)
            //                           .OnDelete(DeleteBehavior.NoAction);


            //modelbuilder.Entity<Chat>().(e => e.Teacher)
            //               .WithMany(e => e.UserID)
            //               .OnDelete(DeleteBehavior.NoAction);



            //modelbuilder.Entity<Chat>().HasOne(e => e.Student)
            //               .WithMany(e => e.Chats)
            //               .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
