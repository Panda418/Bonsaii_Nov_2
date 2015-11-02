 using System;
 using System.Collections.Generic;
 using System.ComponentModel.DataAnnotations;
 using System.ComponentModel.DataAnnotations.Schema;
 using System.Data.Entity.Spatial;

namespace Bonsaii.Models
{
    [Table("StaffApplications")]
    public class StaffApplications
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name="单据类别编号")]
        [StringLength(4)]
        public string BillTypeNumber { get; set; }

        [Required]
        [Display(Name="单据类别名称")]
        [StringLength(10)]
        public string BillTypeName { get; set; }

        [Required]
        [Display(Name="单号")]
        [StringLength(50)]
        public string BillNumber { get; set; }
        [Required]
        [Display(Name = "员工工号")]
        [StringLength(50)]
        public string StaffNumber { get; set; }
        [Display(Name = "姓名")]
        [StringLength(100)]
        public string StaffName { get; set; }
        [Display(Name="申请日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [Column(TypeName = "date")]
        public DateTime? ApplyDate { get; set; }
        [Display(Name = "期望离职日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [Column(TypeName = "date")]
        public DateTime? HopeLeaveDate { get; set; }
        [Display(Name="离职类别")]
        [StringLength(50)]
        public string LeaveType { get; set; }
        [Display(Name = "离职原因")]
        [StringLength(200)]
        public string LeaveReason { get; set; }
        [Display(Name="备注")]
        [StringLength(200)]
        public string Remark { get; set; }
        [Display(Name="状态")]
        [StringLength(50)]
        public string AuditStatus { get; set; }
    }
}
