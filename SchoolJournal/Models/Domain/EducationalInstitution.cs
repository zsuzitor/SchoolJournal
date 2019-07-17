using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolJournal.Data;
using SchoolJournal.Models.Domain.ManyToMany;
using SchoolJournal.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolJournal.Models.Domain
{
    public class EducationalInstitution
    {
        public int Id { get; set; }
        public string Name { get; set; }




        public ApplicationUser Schoolmaster { get; set; }


        public List<Discipline> Disciplines { get; set; }

        // public List<EIStudent> NotApproveStudents { get; set; }

        //public List<EIStudent> Students { get; set; }
        //public List<EIRequestStudent> RequestStudents { get; set; }

        //public List<EITeacher> Teachers { get; set; }
        //public List<EIDeputyPrincipal> DeputyPrincipals { get; set; }
        //public List<EIHeadTeacher> HeadTeachers { get; set; }

        //данные о всех должностях и переходах//актуально только если учебка-школа
        public List<EIUser> EIUsers { get; set; }


        public List<Class> Class { get; set; }

        public EducationalInstitution()
        {
            Disciplines = new List<Discipline>();
            //Students = new List<EIStudent>();
            //Teachers = new List<EITeacher>();
            //DeputyPrincipals = new List<EIDeputyPrincipal>();
            //HeadTeachers = new List<EIHeadTeacher>();
            //RequestStudents = new List<EIRequestStudent>();

            EIUsers = new List<EIUser>();
        }

        public EducationalInstitution(string name, ApplicationUser schoolMaster) : this()
        {
            this.Name = name;
            this.Schoolmaster = schoolMaster;
        }

        public async static Task<EducationalInstitution> ChangeName(int EIId, string name, ApplicationDbContext db)
        {
            var EI = await db.EducationalInstitutions.FirstOrDefaultAsync(x1 => x1.Id == EIId);
            await EI.ChangeName(name, db);
            return EI;
        }

        public async Task ChangeName(string name, ApplicationDbContext db)
        {
            this.Name = name;
            await db.SaveChangesAsync();
        }

        public async Task<List<EIUser>> GetOnRole(AppUserRole role,ApplicationDbContext db)
        {
            return await db.Entry(this).Collection(x1 => x1.EIUsers).Query().Where(x1 => x1.DateEnd == null && x1.Role == role).ToListAsync();
        }
        public async Task<EIUser> UserInActualOnRole(string studentId,AppUserRole role, ApplicationDbContext db)
        {
            return await db.Entry(this).Collection(x1 => x1.EIUsers).Query().Where(x1 => x1.UserId == studentId && x1.DateEnd == null&&x1.Role==role).FirstOrDefaultAsync();
        }

        public async Task<List<EIUser>> GetActualStudents(ApplicationDbContext db)
        {
            return await this.GetOnRole(AppUserRole.Student,db) ;
        }

        //public async Task<List<EIStudent>> GetActualStudents(ApplicationDbContext db)
        //{
        //    return await db.Entry(this).Collection(x1 => x1.Students).Query().Where(x1 => x1.DateEnd == null).ToListAsync();
        //}

        public async Task<EIUser> UserInActualStudents(string studentId,ApplicationDbContext db)
        {
            return await UserInActualOnRole(studentId,AppUserRole.Student,db) ;
        }

        public async Task<List<EIUser>> GetActualRequestStudents(ApplicationDbContext db)
        {
            return await this.GetOnRole(AppUserRole.StudentRequested, db);
        }

        public async Task<EIUser> UserInActualRequestStudents(string studentId, ApplicationDbContext db)
        {
            return await UserInActualOnRole(studentId, AppUserRole.StudentRequested, db);
        }

        public async Task<List<EIUser>> GetActualTeachers(ApplicationDbContext db)
        {
            return await this.GetOnRole(AppUserRole.Teacher, db);
        }

        public async Task<EIUser> UserInActualTeachers(string teacherId, ApplicationDbContext db)
        {
            return await UserInActualOnRole(teacherId, AppUserRole.Teacher, db);
        }

        public async Task<List<EIUser>> GetActualDeputyPrincipals(ApplicationDbContext db)
        {
            return await this.GetOnRole(AppUserRole.DeputyPrincipal, db);
        }


        public async Task<List<Discipline>> AddDisciplineToEI(string[] namesDiscipline,  ApplicationDbContext db)
        {
            List<Discipline> newDiscplines = new List<Discipline>();
            foreach (var i in namesDiscipline)
                newDiscplines.Add(new Discipline(i, this.Id));

            db.Disciplines.AddRange(newDiscplines);
            await db.SaveChangesAsync();
            return newDiscplines;
        }

        public async  Task<bool?> AddUserToEI( string userId, AppUserRole oldRole, AppUserRole newRole, ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            if (newRole == AppUserRole.Admin || oldRole == AppUserRole.Admin)
                return null;

            EIUser containsInUser = await this.UserInActualOnRole(userId, newRole, db);
            EIUser containsInRequest = await this.UserInActualOnRole(userId, oldRole, db);


            if (containsInUser == null && containsInRequest != null)
            {
                ApplicationUser user = await db.Users.FirstOrDefaultAsync(x1 => x1.Id == userId);

                using (var tranzaction = await db.Database.BeginTransactionAsync())
                    try
                    {
                        db.EIUsers.Add(new EIUser(this.Id, userId, newRole));
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

        public async Task<bool?> RemoveUserFromEI( string userId, AppUserRole oldRole, ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            if (oldRole == AppUserRole.Admin)
                return null;
            //
            EIUser containsInUser = await this.UserInActualOnRole(userId, oldRole, db);

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


        public async Task<Class> CreateClass(string name, int number, ApplicationDbContext db)
        {
            Class res = new Domain.Class(name, number,this.Id);
            db.Class.Add(res);
            await db.SaveChangesAsync();
            return res;
        }


    }
}
