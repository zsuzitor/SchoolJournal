﻿using SchoolJournal.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolJournal.Models.Domain.ManyToMany
{
    public class EIUser//: IEIUser
    {

        public int Id { get; set; }

        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        public int? EducationalInstitutionId { get; set; }
        public EducationalInstitution EducationalInstitution { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }


        ////--
        //public bool IsClassroomTeacher { get; set; }
        //public bool IsClassHeadman { get; set; }
        //public bool IsSchoolmaster { get; set; }
        //public bool IsStudentRequested { get; set; }
        //public bool IsTeacherRequested { get; set; }
        //public bool IsStudent { get; set; }
        //public bool IsTeacher { get; set; }
        //public bool IsDeputyPrincipal { get; set; }
        //public bool IsHeadTeacher { get; set; }

        public AppUserRole Role { get; set; }

        //если пользователю необходима связь с классом(он староста\ученик\куратор и тд)
        public int? ClassroomId { get; set; }
        public Class Classroom{ get; set; }

        public EIUser()
        {
            DateStart = DateTime.Now;
            DateEnd = null;
        }

        public EIUser(int EIId, string userId, AppUserRole role) : this()
        {
            //DateStart = DateTime.Now;
            //DateEnd = null;
            UserId = userId;
            EducationalInstitutionId = EIId;
            Role = role;
        }
        public EIUser(int EIId, string userId, int classroomId, AppUserRole role) : this(EIId, userId, role)
        {
            ClassroomId = classroomId;
        }
    }
}
