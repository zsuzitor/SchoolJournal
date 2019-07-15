using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolJournal.Models.SaveModel
{
    //TODO вроде лишняя
    public class EducationalInstitutionSave
    {
        public string NewSchoolmasterId { get; set; }
        public string[] NewDisciplinesName { get; set; }
        public int[] DeleteDisciplines { get; set; }


        public EducationalInstitutionSave()
        {

        }
    }
}
