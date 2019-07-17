using Microsoft.EntityFrameworkCore;
using SchoolJournal.Data;
using SchoolJournal.Models.Domain.ManyToMany;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolJournal.Models.Domain.Users
{
    public class StudentApplicationUser
    {
        ApplicationUser User { get; set; } = null;

        public static bool CheckAccess(AppUserRole role)
        {
            if (role != AppUserRole.Student)
                throw new Exception();
            return true;
        }

        //public static void TestMeth(this ApplicationUser user)
        //{
        //    CheckAccess(user.RoleProp);
        //    //do something
        //}

        public StudentApplicationUser(ApplicationUser user)
        {
            User = user;
        }


        public async static Task<bool> IsHeadman(string studentId, ApplicationDbContext db)
        {
            //var student=await ApplicationUser.GetById(studentId, db);
            var res = StudentApplicationUser.HeadmanRecord(studentId, db);

            return res == null ? false : true;
        }

        public async static Task<EIUser> HeadmanRecord(string studentId, ApplicationDbContext db)
        {
            //var student=await ApplicationUser.GetById(studentId, db);
            return await db.EIUsers.FirstOrDefaultAsync(x1 => x1.UserId == studentId && x1.Role == AppUserRole.ClassHeadman && x1.DateEnd == null);
        }

    }
}
