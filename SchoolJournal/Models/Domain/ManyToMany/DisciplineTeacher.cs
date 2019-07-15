using SchoolJournal.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolJournal.Models.Domain.ManyToMany
{
    public class DisciplineTeacher
    {
        public int Id { get; set; }

        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        public string TeacherId { get; set; }
        public ApplicationUser Teacher { get; set; }
        public int DisciplineId { get; set; }
        public Discipline Discipline { get; set; }

        public DisciplineTeacher()
        {
            DateStart = DateTime.Now;
            DateEnd = null;
        }
    }
}
