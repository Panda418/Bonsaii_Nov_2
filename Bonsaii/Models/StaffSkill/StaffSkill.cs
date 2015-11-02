using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    [Table("StaffSkills")]
    public class StaffSkill
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "单据类别")]
        public string BillType { get; set; }
        [Required]
        [Display(Name = "单号")]
        public string BillNumber { get; set; }
        [Required]
        [Display(Name = "员工工号")]
        public string StaffNumber { get; set; }

        [Display(Name = "姓名")]
        public string StaffName { get; set; }
        [Required]
        [Display(Name = "技能编号")]
        public string SkillNumber { get; set; }

        [Display(Name = "技能名称")]
        public string SkillName { get; set; }
        [Required]
        [Display(Name = "技能级别")]
        public int SkillGrade { get; set; }
        [Display(Name = "备注")]
        public string SkillRemark { get; set; }

    }
}