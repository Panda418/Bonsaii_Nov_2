using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace Bonsaii.Models
{
    [Table("BillProperties")]
    public partial class BillPropertyModels
    {
        public int Id { get; set; }

        [Display(Name="单据类别")]
        public string BillSort { get; set; }

        [Display(Name = "单据性质")]
        public string Type { get; set; }
        [Required]
        [Display(Name = "单据性质名称")]
        [StringLength(50)]
        public string TypeName { get; set; }
        [Display(Name = "单据性质全称")]
        [StringLength(50)]
        public string TypeFullName { get; set; }

        [Display(Name = "单据编码方式")]
        public string CodeMethod { get; set; }

        [StringLength(10)]
        [Display(Name = "编码形式")]
        public string Code { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }

        public int SerialNumber { get; set; }
        [Display(Name = "自动审核")]
        public bool IsAutoAudit { get; set; }
        [Display(Name = "走审批流程")]
        public bool IsApprove { get; set; }
        [Display(Name = "单据限定输入用户")]
        public bool IsLimitInput { get; set; }
        [Display(Name = "增加减少")]
        public bool IsAscOrDesc { get; set; }

        public int Count { get; set; }
    }
}
