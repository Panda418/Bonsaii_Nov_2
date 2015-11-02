 using System;
 using System.Collections.Generic;
 using System.ComponentModel.DataAnnotations;
 using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace Bonsaii.Models
{
    [Table("StaffArchives")]
    public class StaffArchive
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "单据类别编号")]
        [StringLength(4)]
        public string BillTypeNumber { get; set; }

        [Required]
        [Display(Name = "单据类别名称")]
        [StringLength(10)]
        public string BillTypeName { get; set; }

        [Required]
        [Display(Name = "单号")]
        [StringLength(50)]
        public string BillNumber { get; set; }

        [Required]
        [Display(Name = "员工工号")]
        [StringLength(50)]
        public string StaffNumber { get; set; }

        [Required]
        [Display(Name = "姓名")]
        [StringLength(50)]
        public string StaffName { get; set; }

        [Display(Name = "离职日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [Column(TypeName = "date")]
        public DateTime? LeaveDate { get; set; }

        [Required]
        [Display(Name = "再次入职日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [Column(TypeName = "date")]
        public DateTime? ReApplyDate { get; set; }
        [Display(Name = "备注")]
        [StringLength(200)]
        public string Remark { get; set; }
    }
}
