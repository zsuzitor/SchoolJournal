using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolJournal.Models.Domain.ManyToMany
{
    public class ClassLesson
    {
        public int Id { get; set; }

        public int? ClassId { get; set; }
        public Class Class { get; set; }
        public int? LessonId { get; set; }
        public Lesson Lesson { get; set; }


        public ClassLesson()
        {

        }

    }
}
