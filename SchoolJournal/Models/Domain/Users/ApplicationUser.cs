using Microsoft.AspNetCore.Identity;
using SchoolJournal.Models.Domain.ManyToMany;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolJournal.Models.Domain.Users
{

    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public DateTime Birthday { get; set; }
        public bool Male { get; set; }


        //public AppUserRole RoleProp { get; set; }//тк может и учиться и преподавать, надо использовать роли

        //если пользователь классный руководитель
        public int? ClassroomTeacherId { get; set; }
        public Class ClassroomTeacher { get; set; }

        //пользователь староста
        public int? ClassHeadmanId { get; set; }
        public Class ClassHeadman { get; set; }

        //класс пользователя
        public int? ClassId { get; set; }
        public Class Class { get; set; }

        //если пользователь директор
        public int EducationalInstitutionId { get; set; }
        public EducationalInstitution EducationalInstitution { get; set; }//EI


        //EI-EducationalInstitution 

        //EITeacher

        //TODO многие ко многим, учебки в которые подал заявления
        public List<EIRequestStudent> EIRequest { get; set; }

        //TODO многие ко многим, учебки в которых студент учится
        public List<EIStudent> EIStudents { get; set; }

        //TODO многие ко многим, учебки в которых преподает
        public List<EITeacher> EITeachers { get; set; }

        //TODO многие ко многим, учебки в которых зам директора
        public List<EIDeputyPrincipal> EIDeputyPrincipals { get; set; }

        //TODO многие ко многим, учебки в которых завуч
        public List<EIHeadTeacher> EIHeadTeachers { get; set; }


        //уроки учителя планируемые
        public List<Lesson> LessonsPlan { get; set; }
        //уроки учителя проведенные
        public List<Lesson> LessonsFact { get; set; }

        //пользователь учитель и ставит оценки
        public List<Mark> MarksRate { get; set; }

        //пользователь студент и получает оценки
        public List<Mark> MarksReceived { get; set; }

        //пользователь преподаватель и преподает эти дисциплины
        //public List<Discipline> Disciplines { get; set; }
        public List<DisciplineTeacher> DisciplineTeacher { get; set; }


        //пользователь студент и это его оценки
        public List<StudentsPresence> Presences { get; set; }

        public List<VisitBuilding> VisitBuilding { get; set; }


        public ApplicationUser()
        {
            //RoleProp = AppUserRole.WhoIam;
            EIStudents = new List<EIStudent>();
            EIRequest = new List<EIRequestStudent>();
            EITeachers = new List<EITeacher>();
            EIDeputyPrincipals = new List<EIDeputyPrincipal>();
            EIHeadTeachers = new List<EIHeadTeacher>();
            LessonsPlan = new List<Lesson>();
            LessonsFact = new List<Lesson>();
            MarksRate = new List<Mark>();
            MarksReceived = new List<Mark>();
            DisciplineTeacher = new List<DisciplineTeacher>();
            Presences = new List<StudentsPresence>();
            VisitBuilding = new List<VisitBuilding>();
        }



        public async Task<List<string>> GetRoles(UserManager<ApplicationUser> userManager)
        {
            return (await userManager.GetRolesAsync(this)).ToList();
        }


    }

}
