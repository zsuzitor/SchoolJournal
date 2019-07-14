using SchoolJournal.Models.Domain.ManyToMany;
using SchoolJournal.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolJournal.Models.Domain
{
    public class EducationalInstitution
    {
        public int Id { get; set; }
        public string Name { get; set; }




        public ApplicationUser Schoolmaster { get; set; }


        public List<Discipline> Disciplines { get; set; }
        public List<EIStudent> Students { get; set; }
        public List<EITeacher> Teachers { get; set; }
        public List<EIDeputyPrincipal> DeputyPrincipals { get; set; }
        public List<EIHeadTeacher> HeadTeachers { get; set; }

        
            

    }
}
