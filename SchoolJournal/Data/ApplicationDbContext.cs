using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolJournal.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolJournal.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>//: IdentityDbContext<ApplicationUser>
    {
        //public DbSet<Article> Articles { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
               : base(options)
        {
            Database.EnsureCreated();//создаст БД если ее нет
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // использование Fluent API см по тегу fluent
            base.OnModelCreating(modelBuilder);
        }

    }
}
