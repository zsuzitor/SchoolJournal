using SchoolJournal.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolJournal.Models.Domain
{
    public class StudentsPresence
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }//доп инфа по желанию
        public string Remarks { get; set; }//будет отправлено родителям

        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }
        public List<Mark> Marks { get; set; }

        public int PresenceId { get; set; }
        public PresenceStudent Presence { get; set; }

        //урок на котором отмечаем
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

        public StudentsPresence()
        {

        }

    }
}
