using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolJournal.Models.Domain;
using SchoolJournal.Models.Domain.ManyToMany;
using SchoolJournal.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolJournal.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>//: IdentityDbContext<ApplicationUser>
    {
        public DbSet<Class> Class { get; set; }
        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<EducationalInstitution> EducationalInstitutions { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<StudentsPresence> StudentsPresences { get; set; }
        public DbSet<VisitBuilding> VisitsBuildings { get; set; }

        //many to many
        public DbSet<ClassLesson> ClassLessons { get; set; }
        public DbSet<DisciplineTeacher> DisciplineTeachers { get; set; }
        //public DbSet<EIDeputyPrincipal> EIDeputyPrincipals { get; set; }
        //public DbSet<EIHeadTeacher> EIHeadTeachers { get; set; }
        //public DbSet<EIStudent> EIStudents { get; set; }
        //public DbSet<EITeacher> EITeachers { get; set; }

        public DbSet<EIUser> EIUserSchools { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
               : base(options)
        {
            Database.EnsureCreated();//создаст БД если ее нет
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // использование Fluent API см по тегу fluent



            //modelBuilder.Entity<ApplicationUser>().
            //    HasOne(x1 => x1.ClassroomTeacher).
            //    WithOne(x1 => x1.ClassroomTeacher);//.OnDelete(DeleteBehavior.SetNull)

            //modelBuilder.Entity<ApplicationUser>().
            //    HasOne(x1 => x1.ClassHeadman).
            //    WithOne(x1 => x1.Headman);

            modelBuilder.Entity<ApplicationUser>().
                HasOne(x1 => x1.Class).
                WithMany(x1 => x1.Students);

            modelBuilder.Entity<ApplicationUser>().
                HasMany(x1 => x1.LessonsPlan).
                WithOne(x1 => x1.TeacherPlan);

            modelBuilder.Entity<ApplicationUser>().
                HasMany(x1 => x1.LessonsFact).
                WithOne(x1 => x1.TeacherFact);

            modelBuilder.Entity<ApplicationUser>().
                HasMany(x1 => x1.LessonsFact).
                WithOne(x1 => x1.TeacherFact);

            modelBuilder.Entity<ApplicationUser>().
               HasMany(x1 => x1.MarksRate).
               WithOne(x1 => x1.Teacher);

            modelBuilder.Entity<ApplicationUser>().
               HasMany(x1 => x1.MarksReceived).
               WithOne(x1 => x1.Student);

            //TODO user--public List<Discipline> Disciplines { get; set; }
            modelBuilder.Entity<DisciplineTeacher>().
               HasOne(sc => sc.Teacher).
               WithMany(s => s.DisciplineTeacher);
            modelBuilder.Entity<DisciplineTeacher>().
               HasOne(sc => sc.Discipline).
               WithMany(s => s.DisciplineTeacher);

            modelBuilder.Entity<ApplicationUser>().
               HasMany(x1 => x1.Presences).
               WithOne(x1 => x1.Student);

            modelBuilder.Entity<ApplicationUser>().
              HasMany(x1 => x1.VisitBuilding).
              WithOne(x1 => x1.User);

            //TODO class-public List<Lesson> Lessons { get; set; }
            modelBuilder.Entity<ClassLesson>().
                HasOne(sc => sc.Class).
                WithMany(s => s.ClassLesson);
            modelBuilder.Entity<ClassLesson>().
               HasOne(sc => sc.Lesson).
               WithMany(s => s.ClassLesson);


            modelBuilder.Entity<Lesson>().
               HasOne(x1 => x1.Discipline).
               WithMany(x1 => x1.Lessons);

            modelBuilder.Entity<Lesson>().
              HasMany(x1 => x1.StudentsPresence).
              WithOne(x1 => x1.Lesson);

            //


            modelBuilder.Entity<Mark>().
             HasOne(x1 => x1.StudentsPresence).
             WithMany(x1 => x1.Marks);


            //связи с EducationalInstitution
            //modelBuilder.Entity<EducationalInstitution>().
            // HasOne(x1 => x1.Schoolmaster).
            // WithOne(x1 => x1.EducationalInstitution);

            modelBuilder.Entity<EducationalInstitution>().
            HasMany(x1 => x1.Disciplines).
            WithOne(x1 => x1.EducationalInstitution);

            //modelBuilder.Entity<EIStudent>().
            // HasOne(x1 => x1.User).
            // WithMany(x1 => x1.EIStudents);
            //modelBuilder.Entity<EIStudent>().
            // HasOne(x1 => x1.EducationalInstitution).
            // WithMany(x1 => x1.Students);

            //modelBuilder.Entity<EIRequestStudent>().
            //HasOne(x1 => x1.User).
            //WithMany(x1 => x1.EIRequest);
            //modelBuilder.Entity<EIRequestStudent>().
            // HasOne(x1 => x1.EducationalInstitution).
            // WithMany(x1 => x1.RequestStudents);

            //modelBuilder.Entity<EITeacher>().
            //HasOne(x1 => x1.User).
            //WithMany(x1 => x1.EITeachers);
            //modelBuilder.Entity<EITeacher>().
            // HasOne(x1 => x1.EducationalInstitution).
            // WithMany(x1 => x1.Teachers);

           // modelBuilder.Entity<EIHeadTeacher>().
           //HasOne(x1 => x1.User).
           //WithMany(x1 => x1.EIHeadTeachers);
           // modelBuilder.Entity<EIHeadTeacher>().
           //  HasOne(x1 => x1.EducationalInstitution).
           //  WithMany(x1 => x1.HeadTeachers);

           // modelBuilder.Entity<EIDeputyPrincipal>().
           //HasOne(x1 => x1.User).
           //WithMany(x1 => x1.EIDeputyPrincipals);
           // modelBuilder.Entity<EIDeputyPrincipal>().
           //  HasOne(x1 => x1.EducationalInstitution).
           //  WithMany(x1 => x1.DeputyPrincipals);


            modelBuilder.Entity<EIUser>().
           HasOne(x1 => x1.User).
           WithMany(x1 => x1.EIUsers);
            modelBuilder.Entity<EIUser>().
             HasOne(x1 => x1.EducationalInstitution).
             WithMany(x1 => x1.EIUsers);

            //---

            base.OnModelCreating(modelBuilder);
        }

    }
}
