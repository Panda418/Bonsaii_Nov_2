using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    [Table("SkillParameters")]
    public class SkillParameter
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name="技能编号")]
        public string SkillNumber { get; set; }
        [Required]
        [Display(Name="技能名称")]
        public string SkillName { get; set; }
    }
}