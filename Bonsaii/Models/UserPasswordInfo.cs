namespace Bonsaii.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("UserPasswordInfo")]
    public partial class UserPasswordInfo
    {
        [Key]
        [Required]
        public int Id { get; set; }
        
        [Required]
        [StringLength(256)]
        [Display(Name="用户名")]
        [RegularExpression("^1[2,3,4,5,6,7,8][0-9]{9}$", ErrorMessage = "请输入合法的{0}")]
        public string UserName { get; set; }
        
        [Required]
        [StringLength(50)]
        [Display(Name="企业编号")]
   
        public string CompanyId { get; set; }
        
        [Column(TypeName = "image")]
        [Display(Name="营业执照")]
        public byte[] BusinessLicense { get; set; }

  
        [HiddenInput(DisplayValue = false)]
        [StringLength(50)]
        public string BusinessLicenseType { get; set; }

        [Required]
        [StringLength(12)]
        [Display(Name="联系电话")]
        public string TelNumber { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime SubmitTime { get; set; }
        
        [StringLength(50)]
        public string AuditStatus { get; set; }
    }
}
