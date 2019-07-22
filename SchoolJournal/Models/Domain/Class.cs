using Microsoft.EntityFrameworkCore;
using SchoolJournal.Data;
using SchoolJournal.Models.Domain.ManyToMany;
using SchoolJournal.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolJournal.Models.Domain
{
    public class Class
    {
        [Key]
        public int Id { get; set; }
        public int Number { get; set; }//1,2-11
        public string Name { get; set; }//a,b,c

        public bool Actual { get; set; }//false->класс выпустился\расформирован и тд

        //public string ClassroomTeacherId { get; set; }
        //public ApplicationUser ClassroomTeacher { get; set; }

        //староста
        //public string HeadmanId { get; set; }
        //public ApplicationUser Headman { get; set; }

        //студенты
        public List<EIUser> Users { get; set; }

        //public List<Lesson> Lessons { get; set; }
        public List<ClassLesson> ClassLesson { get; set; }

        public int EducationalInstitutionId { get; set; }
        public EducationalInstitution EducationalInstitution { get; set; }

        public Class()
        {
            Users = new List<EIUser>();
            ClassLesson = new List<ClassLesson>();
            Actual = true;
        }

        public Class(string name,int number,int educationalInstitutionId) :this()
        {
            Name = name;
            Number = number;
            EducationalInstitutionId = educationalInstitutionId;
        }


        //для тех записей которые могут встречаться только 1 раз(староста\куратор, тк может быть только 1 для класса)
        public async Task<bool> SetUserSolo( string userId,AppUserRole role, ApplicationDbContext db)
        {
            var eiUser = await db.EIUsers.FirstOrDefaultAsync(x1 => x1.ClassroomId == this.Id && x1.Role == role && x1.DateEnd == null&&x1.UserId==userId);
            if (eiUser != null)
                return false;
            //eiClassroomTeacher.DateEnd = DateTime.Now;
            eiUser = new EIUser(this.EducationalInstitutionId, userId, this.Id, AppUserRole.ClassroomTeacher);
            db.EIUsers.Add(eiUser);
            await db.SaveChangesAsync();
            return true;

        }
        //для тех записей которые могут встречаться только 1 раз(староста\куратор, тк может быть только 1 для класса)
        public async Task<bool> ClearUserSolo(string userId, AppUserRole role, ApplicationDbContext db)
        {
            var eiUser = await db.EIUsers.FirstOrDefaultAsync(x1 => x1.ClassroomId == this.Id && x1.Role == role && x1.DateEnd == null && x1.UserId == userId);
            if (eiUser == null)
                return false;
            eiUser.DateEnd = DateTime.Now;
            await db.SaveChangesAsync();
            return true;
        }


        public async Task<bool> SetHeadman(int eiid,string studentId,ApplicationDbContext db)
        {
            return await this.SetUserSolo(studentId,AppUserRole.ClassHeadman,db);
            //проверить является ли уже старостой
            //if (await StudentApplicationUser.IsHeadman(studentId, db))
            //    return false;
            //db.EIUsers.Add(new EIUser(eiid, studentId, AppUserRole.ClassHeadman) { ClassroomId=this.Id });
            //await db.SaveChangesAsync();
            //return true;

        }

        public async Task<bool> ClearHeadman( string studentId, ApplicationDbContext db)
        {
            return await this.ClearUserSolo(studentId, AppUserRole.ClassHeadman, db);

            //проверить является ли уже старостой
            //var headmanRecord=await StudentApplicationUser.HeadmanRecord(studentId, db);
            //if (headmanRecord == null)
            //    return false;
            //headmanRecord.DateEnd=DateTime.Now;
            //await db.SaveChangesAsync();
            //return true;

        }

        public async Task<bool> SetClassroomTeacher(string classroomTeacherId, ApplicationDbContext db)
        {
            return await this.SetUserSolo(classroomTeacherId, AppUserRole.ClassroomTeacher, db);
            //var eiClassroomTeacher = await db.EIUsers.FirstOrDefaultAsync(x1=>x1.ClassroomId==this.Id&&x1.Role==AppUserRole.ClassroomTeacher && x1.DateEnd == null);
            //if (eiClassroomTeacher != null)
            //    return false;
            ////eiClassroomTeacher.DateEnd = DateTime.Now;
            //eiClassroomTeacher = new EIUser(this.EducationalInstitutionId,classroomTeacherId, this.Id, AppUserRole.ClassroomTeacher );
            //db.EIUsers.Add(eiClassroomTeacher);
            //await db.SaveChangesAsync();
            //return true;
        }

        public async Task<bool> ClearClassroomTeacher(string studentId, ApplicationDbContext db)
        {
            return await this.ClearUserSolo(studentId, AppUserRole.ClassroomTeacher, db);
            //var eiClassroomTeacher = await db.EIUsers.FirstOrDefaultAsync(x1 => x1.ClassroomId == this.Id && x1.Role == AppUserRole.ClassroomTeacher&&x1.DateEnd==null);
            //if (eiClassroomTeacher == null)
            //    return false;
            //eiClassroomTeacher.DateEnd = DateTime.Now;
            //await db.SaveChangesAsync();
            //return true;
        }



        public async Task<bool> AddStudent(string studentId, ApplicationDbContext db)
        {
        }
        public async Task<bool> RemoveStudent(string studentId, ApplicationDbContext db)
        {
        }

        public async Task<bool> AddLesson(string studentId, ApplicationDbContext db)
        {
        }

    }
}
