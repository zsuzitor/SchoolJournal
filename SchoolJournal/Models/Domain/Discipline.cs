using Microsoft.EntityFrameworkCore;
using SchoolJournal.Data;
using SchoolJournal.Models.Domain.ManyToMany;
using SchoolJournal.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolJournal.Models.Domain
{
    //математика\физика и тд
    public class Discipline
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        //public bool Actual { get; set; }//если false то нельзя добавлять занятия и тд

        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        public int EducationalInstitutionId { get; set; }
        public EducationalInstitution EducationalInstitution { get; set; }

        //public List<ApplicationUser> Techers { get; set; }
        public List<DisciplineTeacher> DisciplineTeachers { get; set; }

        public List<Lesson> Lessons { get; set; }

        public Discipline()
        {
            DateStart = DateTime.Now;
            DateEnd = null;

            DisciplineTeachers = new List<DisciplineTeacher>();
            Lessons = new List<Lesson>();
            //Actual = true;
        }

        public Discipline(string name, int educationalInstitutionId) : this()
        {
            this.Name = name;
            EducationalInstitutionId = educationalInstitutionId;
        }





        public async Task SetNotActual(ApplicationDbContext db)
        {
            this.DateEnd = DateTime.Now;
            await db.SaveChangesAsync();
            return;
        }



        public async Task<DisciplineTeacher> TeacherInActuals(string userId, ApplicationDbContext db)
        {
            return await db.Entry(this).Collection(x1 => x1.DisciplineTeachers).Query().Where(x1 => x1.TeacherId == userId && x1.DateEnd == null).FirstOrDefaultAsync();
        }

        public async Task<bool?> AddTeacherToDiscipline(EducationalInstitution ei, string teacherId, ApplicationDbContext db)
        {
            if (ei.Id != this.EducationalInstitutionId)
                return null;

            //проверить есть ли он уже в дисциплине
            //есть ли он в учителях


            DisciplineTeacher discpTeahcer = await this.TeacherInActuals(teacherId, db);
            if (discpTeahcer != null)
                return null;
            var eiUeser = await ei.UserInActualTeachers(teacherId, db);
            if (eiUeser == null)
                return null;
            db.DisciplineTeachers.Add(new DisciplineTeacher(teacherId, this.Id));
            await db.SaveChangesAsync();
            return true;
        }


        public async Task<bool?> RemoveTeacherFromDiscipline(EducationalInstitution ei, string teacherId, ApplicationDbContext db)
        {
            if (ei.Id != this.EducationalInstitutionId)
                return null;

            //проверить есть ли он уже в дисциплине

            DisciplineTeacher discpTeahcer = await this.TeacherInActuals(teacherId, db);
            if (discpTeahcer == null)
                return null;
            discpTeahcer.DateEnd = DateTime.Now;
            await db.SaveChangesAsync();
            return true;
        }


    }
}
