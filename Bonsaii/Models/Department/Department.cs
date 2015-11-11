using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{  
    [Table("Departments")]
    public class Department
    {
            [Key]
            public int Id { get; set; }

            [Display(Name = "部门编号")]
            public string DepartmentId { get; set; }

            //[Required]
            [Display(Name = "部门名称")]
            public string Name { get; set; }

            [Display(Name = "上级部门")]
            public string ParentDepartmentId { get; set; }
            [Display(Name = "编制人数")]
            public int StaffSize { get; set; }

            [Display(Name = "备注")]
          
            public string Remark { get; set; }
    }
}