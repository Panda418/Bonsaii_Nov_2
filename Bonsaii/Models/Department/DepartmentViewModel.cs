using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    public class DepartmentViewModel
    {

        public int Id { get; set; }
        [Display(Name = "部门ID")]
        public string Number { get; set; }
        
        [Display(Name = "部门名称")]
        public string Name { get; set; }

        [Display(Name = "上级部门")]
        public string ParentDepartmentName { get; set; }
        [Display(Name = "编制人数")]
        public int StaffSize { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }

        public string Description { get; set; }
        public string Value { get; set; }
    }
}