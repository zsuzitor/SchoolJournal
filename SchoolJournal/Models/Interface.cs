using SchoolJournal.Models.Domain;
using SchoolJournal.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolJournal.Models
{
    public class Interface
    {
    }



    public interface IEIUser
    {
         int Id { get; set; }

         DateTime DateStart { get; set; }
         DateTime? DateEnd { get; set; }

         int EducationalInstitutionId { get; set; }
         EducationalInstitution EducationalInstitution { get; set; }

         string UserId { get; set; }
         ApplicationUser User { get; set; }
    }
}
