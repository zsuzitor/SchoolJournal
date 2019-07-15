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

        public List<EIStudent> Students { get; set; }
        public List<EIRequestStudent> RequestStudents { get; set; }

        public List<EITeacher> Teachers { get; set; }
        public List<EIDeputyPrincipal> DeputyPrincipals { get; set; }
        public List<EIHeadTeacher> HeadTeachers { get; set; }


        public EducationalInstitution()
        {
            Disciplines = new List<Discipline>();
            Students = new List<EIStudent>();
            Teachers = new List<EITeacher>();
            DeputyPrincipals = new List<EIDeputyPrincipal>();
            HeadTeachers = new List<EIHeadTeacher>();
            RequestStudents = new List<EIRequestStudent>();
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


        public async Task<List<EIStudent>> GetActualStudents(ApplicationDbContext db)
        {
            return await db.Entry(this).Collection(x1 => x1.Students).Query().Where(x1 => x1.DateEnd == null).ToListAsync();
        }

        public async Task<EIStudent> UserInActualStudents(string studentId,ApplicationDbContext db)
        {
            return await db.Entry(this).Collection(x1 => x1.Students).Query().Where(x1 =>  x1.UserId == studentId && x1.DateEnd == null).FirstOrDefaultAsync();
        }

        public async Task<List<EIRequestStudent>> GetActualRequestStudents(ApplicationDbContext db)
        {
            return await db.Entry(this).Collection(x1 => x1.RequestStudents).Query().Where(x1 => x1.DateEnd == null).ToListAsync();
        }

        public async Task<EIRequestStudent> UserInActualRequestStudents(string studentId, ApplicationDbContext db)
        {
            return await db.Entry(this).Collection(x1 => x1.RequestStudents).Query().Where(x1 => x1.UserId == studentId && x1.DateEnd == null).FirstOrDefaultAsync();
        }

        public async Task<List<EITeacher>> GetActualTeachers(ApplicationDbContext db)
        {
            return await db.Entry(this).Collection(x1 => x1.Teachers).Query().Where(x1 => x1.DateEnd == null).ToListAsync();
        }

        public async Task<EITeacher> UserInActualTeachers(string studentId, ApplicationDbContext db)
        {
            return await db.Entry(this).Collection(x1 => x1.Teachers).Query().Where(x1 => x1.UserId == studentId && x1.DateEnd == null).FirstOrDefaultAsync();
        }

        public async Task<List<EIDeputyPrincipal>> GetActualDeputyPrincipals(ApplicationDbContext db)
        {
            return await db.Entry(this).Collection(x1 => x1.DeputyPrincipals).Query().Where(x1 => x1.DateEnd == null).ToListAsync();
        }

        public async Task<EITeacher> UserInActualTeachers(string studentId, ApplicationDbContext db)
        {
            return await db.Entry(this).Collection(x1 => x1.Teachers).Query().Where(x1 => x1.UserId == studentId && x1.DateEnd == null).FirstOrDefaultAsync();
        }

        public async Task<List<EIHeadTeacher>> GetActualHeadTeachers(ApplicationDbContext db)
        {
            return await db.Entry(this).Collection(x1 => x1.HeadTeachers).Query().Where(x1 => x1.DateEnd == null).ToListAsync();
        }

    }
}
