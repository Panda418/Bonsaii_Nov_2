namespace Bonsaii.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aa : DbMigration
    {
        public override void Up()
        {
           
            
            CreateTable(
                "dbo.RecordDatetimes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Recordtime = c.DateTime(nullable: false),
                        Tag = c.String(),
                        Month = c.Int(nullable: false),
                        Day = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        
            
          
            
            CreateTable(
                "dbo.WeekTags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nian = c.String(),
                        Range = c.String(),
                        Week1 = c.String(),
                        Week2 = c.String(),
                        Week3 = c.String(),
                        Week4 = c.String(),
                        Week5 = c.String(),
                        Week6 = c.String(),
                        Week7 = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StaffParam", "StaffParamTypeId", "dbo.StaffParamType");
            DropIndex("dbo.StaffParam", new[] { "StaffParamTypeId" });
            DropTable("dbo.WeekTags");
            DropTable("dbo.VerifyCode");
            DropTable("dbo.Users");
            DropTable("dbo.UserRoles");
            DropTable("dbo.Staffs");
            DropTable("dbo.StaffParamType");
            DropTable("dbo.StaffParam");
            DropTable("dbo.StaffChanges");
            DropTable("dbo.StaffBasicParam");
            DropTable("dbo.StaffArchives");
            DropTable("dbo.StaffApplications");
            DropTable("dbo.SkillParameters");
            DropTable("dbo.Recruitments");
            DropTable("dbo.RecordDatetimes");
            DropTable("dbo.PhraseScenes");
            DropTable("dbo.Phrases");
            DropTable("dbo.Params");
            DropTable("dbo.ParamCodes");
            DropTable("dbo.Nations");
            DropTable("dbo.Nationalities");
            DropTable("dbo.Holidaies");
            DropTable("dbo.Healths");
            DropTable("dbo.GroupCompanies");
            DropTable("dbo.Departments");
            DropTable("dbo.Companies");
            DropTable("dbo.BillProperties");
            DropTable("dbo.Backgrounds");
        }
    }
}
