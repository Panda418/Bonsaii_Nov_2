using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    public class SystemDbContext : DbContext
    {
         public SystemDbContext()
            : base("DefaultConnection")
        {
        }

        //public static ApplicationDbContext Create()
        //{
        //    return new ApplicationDbContext();
        //}

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            string name = "STOP HERE";
        }

        public DbSet<Company> Companies { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.GroupCompany> GroupCompanies { get; set; }

        public DbSet<VerifyCode> VerifyCodes { get; set; }

        public DbSet<UserModels> Users { get; set; }

        public DbSet<Actions> Actions { get; set; }


        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RoleModels> Roles { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.UserViewModels> UserViewModels { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.StaffChange> StaffChanges { get; set; }

        public System.Data.Entity.DbSet<Bonsaii.Models.UserPasswordInfo> UserPasswordInfos { get; set; }
    }
}