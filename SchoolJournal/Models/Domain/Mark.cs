using SchoolJournal.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolJournal.Models.Domain
{
    public class Mark
    {
        [Key]
        public int Id { get; set; }

        public string TeacherId { get; set; }
        public ApplicationUser Teacher { get; set; }

        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }
        public int Num { get; set; }
        public DateTime Date { get; set; }

        public int StudentsPresenceId { get; set; }
        public StudentsPresence StudentsPresence { get; set; }


    }
}
