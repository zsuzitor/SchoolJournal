using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolJournal.Data;
using SchoolJournal.Models.Domain.ManyToMany;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolJournal.Models.Domain.Users
{

    //директор
    public class SchoolmasterApplicationUser
    {
        ApplicationUser User { get; set; } = null;

        public static bool CheckAccess(AppUserRole role)
        {
            if (role != AppUserRole.Schoolmaster)
                throw new Exception();
            return true;
        }

        //public static void TestMeth(this ApplicationUser user)
        //{
        //    CheckAccess(user.RoleProp);
        //    //do something
        //}

        public SchoolmasterApplicationUser(ApplicationUser user)
        {
            User = user;
        }


        //public async Task EditEI(EducationalInstitution newEI,ApplicationDbContext db)
        //{
        //    await db.Entry<ApplicationUser>(this.User).Reference(x1 => x1.EducationalInstitution).LoadAsync();
        //    this.User.EducationalInstitution

        //}

        //получить учебку в которой он является директором
        public async static Task<EducationalInstitution> GetEISchoolmaster(string schoolmasterId, ApplicationDbContext db)
        {
            var eIId = (await db.EIUsers.Where(x1 => x1.UserId == schoolmasterId && x1.DateEnd == null && x1.Role == AppUserRole.Schoolmaster).FirstOrDefaultAsync())?.EducationalInstitutionId;
            if (eIId == null)
                return null;
            return await db.EducationalInstitutions.FirstOrDefaultAsync(x1 => x1.Id == eIId);


        }


        public async static Task<List<Discipline>> AddDisciplineToEI(string schoolmasterId, string[] namesDiscipline, ApplicationDbContext db, EducationalInstitution ei = null)
        {
            ei = ei ?? await SchoolmasterApplicationUser.GetEISchoolmaster(schoolmasterId, db);
            if (ei == null)
                return null;
            return await ei.AddDisciplineToEI(namesDiscipline, db);
        }

        //сделать не актуальной
        public async static Task<Discipline> SetNotActualDisciplineToEI(string schoolmasterId, int disciplineId, ApplicationDbContext db)
        {

            //db.Disciplines.RemoveRange(db.Disciplines.Where(x1=> disciplinesId.Contains(x1.Id)));
            var discp = await db.Disciplines.FirstOrDefaultAsync(x1 => x1.Id == disciplineId);
            if (discp == null)
                return null;
            EducationalInstitution ei = await SchoolmasterApplicationUser.GetEISchoolmaster(schoolmasterId, db);
            if (ei == null)
                return null;

            if (discp.EducationalInstitutionId != ei.Id)
                return null;
            await discp.SetNotActual(db);

            return discp;
        }

        //обавить в список студентов
        public async static Task<bool?> AddUserToEI(string schoolmasterId, string userId, AppUserRole oldRole, AppUserRole newRole, ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {

            EducationalInstitution ei = await SchoolmasterApplicationUser.GetEISchoolmaster(schoolmasterId, db);
            if (ei == null)
                return null;
            //
            return await ei.AddUserToEI( userId, oldRole, newRole, db, userManager);
        }

        public async static Task<bool?> RemoveUserFromEI(string schoolmasterId, string userId, AppUserRole oldRole, ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            EducationalInstitution ei = await SchoolmasterApplicationUser.GetEISchoolmaster(schoolmasterId, db);
            if (ei == null)
                return null;
            return await ei.RemoveUserFromEI(userId, oldRole, db, userManager);

        }


        //TODO мб надо вынести в завуча еще его куда нибудь
        public async static Task<bool?> AddTeacherToDiscipline(int disciplineId, string schoolmasterId, string teacherId, ApplicationDbContext db)
        {
            var ei = await SchoolmasterApplicationUser.GetEISchoolmaster(schoolmasterId, db);
            if (ei == null)
                return null;

            Discipline discp = await db.Disciplines.FirstOrDefaultAsync(x1 => x1.Id == disciplineId);
            if (discp == null)
                return null;
            return await discp.AddTeacherToDiscipline(ei, teacherId, db);


        }

        public async static Task<Class> CreateClass(string schoolmasterId,string name,int number, ApplicationDbContext db)
        {
            var ei = await SchoolmasterApplicationUser.GetEISchoolmaster(schoolmasterId, db);
            if (ei == null)
                return null;
            var newClass=await ei.CreateClass(name, number,db);
            return newClass;


        }



    }
}
