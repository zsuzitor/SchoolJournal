using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolJournal.Models
{
    public class Enums
    {
    }

    public enum PresenceStudent { Presence, Lateness, Absence };

    public enum LessonStatus { NotStarted,Canceled,Complited};

    public enum AppUserRole
    {
        //HeadTeacherApplicationUser
        WhoIam,
        Student,
        Parent,
        Teacher,
        Schoolmaster,//директор
        Admin,
        DeputyPrincipal,//заместитель директора
        HeadTeacher//завуч
    };


}
