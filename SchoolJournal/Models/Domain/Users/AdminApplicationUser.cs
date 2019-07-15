using Microsoft.EntityFrameworkCore;
using SchoolJournal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolJournal.Models.Domain.Users
{
    public  class AdminApplicationUser
    {
        ApplicationUser User { get; set; } = null;



            public AdminApplicationUser(ApplicationUser user)
        {
            User = user;
        }


        public static bool CheckAccess(AppUserRole role)
        {
            if (role != AppUserRole.Admin)
                throw new Exception();
            return true;
        }

        //public static void TestMeth(this ApplicationUser user)
        //{
        //    CheckAccess(user.RoleProp);
        //    //do something
        //}

        




        public async static Task CreateEducationalInstitution(string name,ApplicationUser schoolMaster,ApplicationDbContext db)
        {
            var EI = new EducationalInstitution(name, schoolMaster);
            db.EducationalInstitutions.Add(EI);
            await db.SaveChangesAsync();

        }

        public async static Task<EducationalInstitution> ChangeNameEI(int EIId,string name, ApplicationDbContext db)
        {
            return await EducationalInstitution.ChangeName(EIId,name,db);
        }

    }
}
