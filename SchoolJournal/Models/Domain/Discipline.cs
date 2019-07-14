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


        public int EducationalInstitutionId { get; set; }
        public EducationalInstitution EducationalInstitution { get; set; }

        //public List<ApplicationUser> Techers { get; set; }
        public List<DisciplineTeacher> DisciplineTeacher { get; set; }

        public List<Lesson> Lessons { get; set; }

    }
}
