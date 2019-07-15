using SchoolJournal.Models.Domain.ManyToMany;
using SchoolJournal.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolJournal.Models.Domain
{
    public class Class
    {
        [Key]
        public int Id { get; set; }
        public int Number { get; set; }//1,2-11
        public string Name { get; set; }//a,b,c

        public bool Actual { get; set; }//false->класс выпустился\расформирован и тд

        public string ClassroomTeacherId { get; set; }
        public ApplicationUser ClassroomTeacher { get; set; }

        //староста
        public string HeadmanId { get; set; }
        public ApplicationUser Headman { get; set; }

        //студенты
        public List<ApplicationUser> Students { get; set; }

        //public List<Lesson> Lessons { get; set; }
        public List<ClassLesson> ClassLesson { get; set; }

        public Class()
        {
            Students = new List<ApplicationUser>();
            ClassLesson = new List<ClassLesson>();
        }

    }
}
