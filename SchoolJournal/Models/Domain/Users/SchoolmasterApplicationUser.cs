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
            if (newRole == AppUserRole.Admin || oldRole == AppUserRole.Admin)
                return null;
            EducationalInstitution ei = await SchoolmasterApplicationUser.GetEISchoolmaster(schoolmasterId, db);
            if (ei == null)
                return null;
            //
            EIUser containsInUser = await ei.UserInActualOnRole(userId, newRole, db);
            EIUser containsInRequest = await ei.UserInActualOnRole(userId, oldRole, db);


            if (containsInUser == null && containsInRequest != null)
            {
                ApplicationUser user = await db.Users.FirstOrDefaultAsync(x1 => x1.Id == userId);

                using (var tranzaction = await db.Database.BeginTransactionAsync())
                    try
                    {
                        db.EIUsers.Add(new EIUser(ei.Id, userId, newRole));
                        containsInRequest.DateEnd = DateTime.Now;

                        await db.SaveChangesAsync();

                        //TODO не понятно что произойдет если роль уже есть, возможно надо это проверять
                        //не обрабатываются некоторые роли типа: TeacherRequested и тд
                        var rolesEI = await ApplicationUser.GetRolesEI(user.Id, db);
                        if (rolesEI.Count(x1 => x1 == oldRole) == 1)
                            await userManager.RemoveFromRoleAsync(user, oldRole.ToString());

                        await userManager.AddToRoleAsync(user, newRole.ToString());

                        tranzaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        tranzaction.Rollback();
                        return null;
                    }
            }
            return false;
        }

        public async static Task<bool?> RemoveUserFromEI(string schoolmasterId, string userId, AppUserRole oldRole, ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            EducationalInstitution ei = await SchoolmasterApplicationUser.GetEISchoolmaster(schoolmasterId, db);
            if (ei == null)
                return null;
            if (oldRole == AppUserRole.Admin)
                return null;
            //
            EIUser containsInUser = await ei.UserInActualOnRole(userId, oldRole, db);

            if (containsInUser != null)
            {
                ApplicationUser user = await db.Users.FirstOrDefaultAsync(x1 => x1.Id == userId);

                containsInUser.DateEnd = DateTime.Now;
                await db.SaveChangesAsync();
                //TODO надо проверять нужно ли удалять роль
                var rolesEI = await ApplicationUser.GetRolesEI(user.Id, db);
                if (rolesEI.Count(x1 => x1 == oldRole) == 1)
                    await userManager.RemoveFromRoleAsync(user, oldRole.ToString());

                return true;
            }

            return false;
        }


    }
}
