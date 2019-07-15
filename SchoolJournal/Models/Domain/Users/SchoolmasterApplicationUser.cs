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

            public async Task LoadEI( ApplicationDbContext db)
        {
            if (this.User.EducationalInstitutionId == 0)
                return;
            await db.Entry(this.User).Reference(x1 => x1.EducationalInstitution).LoadAsync();

        }


        public async Task<List<Discipline>> AddDisciplineToEI(string[]namesDiscipline,ApplicationDbContext db)
        {
            await this.LoadEI(db); 
            if (this.User.EducationalInstitution == null)
                return null;
            return await Discipline.AddDisciplineToEI(namesDiscipline,this.User,db);
        }

        //сделать не актуальной
        public async Task<Discipline> SetNotActualDisciplineToEI(int disciplineId, ApplicationDbContext db)
        {
            
            //db.Disciplines.RemoveRange(db.Disciplines.Where(x1=> disciplinesId.Contains(x1.Id)));
            var discp=await db.Disciplines.FirstOrDefaultAsync(x1=>x1.Id== disciplineId);

            if (discp.EducationalInstitutionId != this.User.EducationalInstitutionId)
                return null;
            await discp.SetNotActual(db);
           
            return discp;
        }


        public async Task<bool?> AddStudentToEI(string studentId, ApplicationDbContext db)
        {
            await this.LoadEI(db);
            if (this.User.EducationalInstitution == null)
                return null;
            //
            EIStudent containsInUser =await this.User.EducationalInstitution.StudentInActualStudents(studentId,db);
            EIRequestStudent containsInRequest = await db.Entry(this.User.EducationalInstitution).Collection(x1 => x1.RequestStudents).Query().
                Where(x1 => x1.UserId == studentId && x1.DateEnd == null).FirstOrDefaultAsync();


            //this.User.EducationalInstitution.Students.Where
            if (containsInUser == null && containsInRequest != null)
                using (var tranzaction = await db.Database.BeginTransactionAsync())
                    try
                    {
                        db.EIStudents.Add(new EIStudent(this.User.EducationalInstitutionId, studentId));
                        containsInRequest.DateEnd = DateTime.Now;

                        await db.SaveChangesAsync();
                        tranzaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        tranzaction.Rollback();
                        return null;
                    }


            return false;
        }

        public async Task<bool?> RemoveStudentToEI(string studentId, ApplicationDbContext db)
        {
            await this.LoadEI(db);
            if (this.User.EducationalInstitution == null)
                return null;
            //
            EIStudent containsInUser = await db.Entry(this.User.EducationalInstitution).Collection(x1 => x1.Students).Query().
                Where(x1 => x1.UserId == studentId && x1.DateEnd == null).FirstOrDefaultAsync();
            //this.User.EducationalInstitution.Students.Where
            if (containsInUser != null)
            {
                containsInUser.DateEnd = DateTime.Now;
                await db.SaveChangesAsync();

                return true;
            }

            return false;
        }


    }
}
