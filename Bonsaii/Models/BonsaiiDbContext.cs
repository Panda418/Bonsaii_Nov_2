using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    public class BonsaiiDbContext : DbContext
    {
        //含有连接字符串参数的构造函数，会连接参数中传递的数据库
        public BonsaiiDbContext(string conn)
            : base(conn)
        {
        }
        public BonsaiiDbContext()
            : base("DefaultConnection")
        { 
        
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public DbSet<Company> Companies { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.GroupCompany> GroupCompanies { get; set; }

        public DbSet<VerifyCode> VerifyCodes { get; set; }

        public DbSet<UserModels> Users { get; set; }


        public DbSet<UserRole> UserRoles { get; set; }
       // public System.Data.Entity.DbSet<Bonsaii.Models.> Departments { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.Phrase> Phrases { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.PhraseScene> PhraseScenes { get; set; }
        public DbSet<Staff> Staffs { get; set; }


        public System.Data.Entity.DbSet<Bonsaii.Models.Nationality> Nationalities { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.Health> Healths { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.Background> Backgrounds { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.Nation> Nations { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.SkillParameter> SkillParameters { get; set; }
        public System.Data.Entity.DbSet<Bonsaii.Models.StaffChange> StaffChanges { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.StaffApplications> StaffApplications { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.StaffArchive> StaffArchives { get; set; }

        public DbSet<BillPropertyModels> BillProperties { get; set; }

        public DbSet<ParamCodes> ParamCodes { get; set; }

        public DbSet<Params> Params { get; set; }

        public DbSet<StaffParam> StaffParams { get; set; }
        public DbSet<StaffParamType> StaffParamTypes { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.StaffBasicParam> StaffBasicParams { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.Recruitments> Recruitments { get; set; }
        public System.Data.Entity.DbSet<Bonsaii.Models.RecordDatetime> RecordDatetimes { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.WeekTag> WeekTags { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.Holiday> Holidays { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.Contract> Contracts { get; set; }
        public System.Data.Entity.DbSet<Bonsaii.Models.StaffSkill> StaffSkills { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.DepartmentReserve> DepartmentReserves { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.ReserveField> ReserveFields { get; set; }
        public System.Data.Entity.DbSet<Bonsaii.Models.StaffSkillReserve> StaffSkillReserves { get; set; }
        public System.Data.Entity.DbSet<Bonsaii.Models.StaffReserve> StaffReserves { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.Train.TrainStart> TrainStarts { get; set; }
        public System.Data.Entity.DbSet<Bonsaii.Models.Department> Departments { get; set; }
    }
}