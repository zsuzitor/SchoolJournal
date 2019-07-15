using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolJournal.Models.Domain.Users
{
    public  class TeacherApplicationUser
    {
        ApplicationUser User { get; set; } = null;

        public static bool CheckAccess(AppUserRole role)
        {
            if (role != AppUserRole.Teacher)
                throw new Exception();
            return true;
        }

        //public static void TestMeth(this ApplicationUser user)
        //{
        //    CheckAccess(user.RoleProp);
        //    //do something
        //}

        public TeacherApplicationUser(ApplicationUser user)
        {
            User = user;
        }
    }
}
