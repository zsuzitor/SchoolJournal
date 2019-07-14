using SchoolJournal.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolJournal.Models.Domain
{
    public class VisitBuilding
    {
        [Key]
        public int Id { get; set; }

        public string Adress { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime Enter { get; set; }
        public DateTime Exit { get; set; }
    }
}
