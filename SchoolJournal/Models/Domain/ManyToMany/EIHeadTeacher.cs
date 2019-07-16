//using SchoolJournal.Models.Domain.Users;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace SchoolJournal.Models.Domain.ManyToMany
//{
//    public class EIHeadTeacher: IEIUser
//    {
//        public int Id { get; set; }

//        public DateTime DateStart { get; set; }
//        public DateTime? DateEnd { get; set; }

//        public int EducationalInstitutionId { get; set; }
//        public EducationalInstitution EducationalInstitution { get; set; }

//        public string UserId { get; set; }
//        public ApplicationUser User { get; set; }

//        public EIHeadTeacher()
//        {
//            DateStart = DateTime.Now;
//            DateEnd = null;
//        }
//    }
//}
