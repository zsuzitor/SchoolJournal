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
        public List<DisciplineTeacher> DisciplineTeacher { get; set; }

        public List<Lesson> Lessons { get; set; }

        public Discipline()
        {
            DateStart = DateTime.Now;
            DateEnd = null;

            DisciplineTeacher = new List<DisciplineTeacher>();
            Lessons = new List<Lesson>();
            //Actual = true;
        }

        public Discipline(string name,int educationalInstitutionId) :this()
        {
            this.Name = name;
            EducationalInstitutionId = educationalInstitutionId;
        }


        public  async static Task<List<Discipline>> AddDisciplineToEI(string[] namesDiscipline,ApplicationUser user, ApplicationDbContext db)
        {
            List<Discipline> newDiscplines = new List<Discipline>();
            foreach (var i in namesDiscipline)
                newDiscplines.Add(new Discipline(i, user.EducationalInstitutionId));

            db.Disciplines.AddRange(newDiscplines);
            await db.SaveChangesAsync();
            return newDiscplines;
        }


        public async Task SetNotActual( ApplicationDbContext db)
        {
            this.DateEnd = DateTime.Now;
            await db.SaveChangesAsync();
            return ;
        }


    }
}
