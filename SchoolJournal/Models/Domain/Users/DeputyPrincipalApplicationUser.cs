using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolJournal.Models.Domain.Users
{
    public static class DeputyPrincipalApplicationUser
    {

        public static bool CheckAccess(AppUserRole role)
        {
            if (role != AppUserRole.DeputyPrincipal)
                throw new Exception();
            return true;
        }

        public static void TestMeth(this ApplicationUser user)
        {
            CheckAccess(user.RoleProp);
            //do something
        }
    }
}
