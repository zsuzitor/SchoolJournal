using SchoolJournal.Models.Domain.ManyToMany;
using SchoolJournal.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolJournal.Models.Domain
{
    public class Lesson
    {
        [Key]
        public int Id { get; set; }

        public string Theme { get; set; }
        public string Homework { get; set; }
        public int Num { get; set; }//номер урока по расписанию
        public LessonStatus Status { get; set; }

        public string TeacherId { get; set; }
        public ApplicationUser TeacherPlan { get; set; }

        public string TeacherFactId { get; set; }
        public ApplicationUser TeacherFact { get; set; }

        public int DisciplineId { get; set; }
        public Discipline Discipline { get; set; }

        public List<StudentsPresence> StudentsPresence { get; set; }

        //public List<Class> Class { get; set; }
        public List<ClassLesson> ClassLesson { get; set; }
    }
}
