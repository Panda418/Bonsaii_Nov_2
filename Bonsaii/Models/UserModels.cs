namespace Bonsaii.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Users")]
    public partial class UserModels
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTime? LockoutEndDateUtc { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        [Display(Name="管理员名称")]
        [StringLength(256)]
        public string UserName { get; set; }

        [Required]
        [StringLength(128)]
        public string Discriminator { get; set; }

        [Display(Name="企业编号")]
        [StringLength(10)]
        public string CompanyId { get; set; }
        [Display(Name="企业全称")]
        public string CompanyFullName { get; set; }

        public string ConnectionString { get; set; }

        [Display(Name="是否有效")]
        public bool IsAvailable { get; set; }
        [Display(Name="审核状态")]
        public bool IsProved { get; set; }

        [Display(Name="注册用户")]
        public bool IsRoot { get; set; }
    }
}
