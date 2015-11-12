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

<<<<<<< HEAD
            [Display(Name = "部门编号")]
            public string DepartmentId { get; set; }

            //[Required]
=======
            [Required]
            [Display(Name = "部门编号")]
            public string DepartmentId { get; set; }

            [Required]
>>>>>>> 7f6daae59d3f52aeb49e3b88babd3194b0c3112d
            [Display(Name = "部门名称")]
            public string Name { get; set; }

            [Display(Name = "上级部门")]
            public string ParentDepartmentId { get; set; }
<<<<<<< HEAD
=======

            [Range(1,5000)]
>>>>>>> 7f6daae59d3f52aeb49e3b88babd3194b0c3112d
            [Display(Name = "编制人数")]
            public int StaffSize { get; set; }

            [Display(Name = "备注")]
          
            public string Remark { get; set; }
    }
}