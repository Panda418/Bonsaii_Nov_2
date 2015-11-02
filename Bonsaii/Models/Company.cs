using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bonsaii.Models
{
    [Table("Companies")]
    public class Company
    {
        [Key]
        [Required]
        [Display(Name="企业编号")]
       public string CompanyId { get; set; }
        [Required]
        [Display(Name = "企业全称")]
        public string FullName { get; set; }
        [Required]
        [Display(Name = "联系电话")]
        public string TelNumber { get; set; }
        [Display(Name = "营业执照")]
        public string BusinessLicense { get; set; }
        [Display(Name="企业用户名")]
        public string UserName { get; set; }
        [Display(Name = "母企业")]
        public string ParentCompany { get; set; }
        [Display(Name = "母企业编号")]
        public string ParentCompanyId { get; set; }
        [Display(Name = "企业简称")]
        public string ShortName { get; set; }
        [Display(Name = "英文名称")]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "请输入正确的英文名称")]
        public string EnglishName { get; set; }
        [Display(Name = "法人代表")]
        public string LegalRepresentative { get; set; }
        [Display(Name = "成立日期")]
        [DataType(DataType.DateTime)]
        public DateTime EstablishDate { get; set; }
        [Display(Name = "电子邮箱")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+[A-Za-z]{2,4}", ErrorMessage = "电子邮箱是无效的")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "联系地址")]
        public string Address { get; set; }
        [Display(Name = "企业网址")]
        [Url]
        public string Url { get; set; }
        [Display(Name = "LOGO")]
        public byte[] Logo { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string LogoType { get; set; }
        [Display(Name = "备注")]
        public string Remark { get; set; }
        [Display(Name = "是否集团企业")]
        public Boolean IsGroupCompany { get; set; }
        [Display(Name = "集团企业编号")]
        public string GroupCompanyNumber { get; set; }


    }
    [Table("GroupCompanies")]
    public class GroupCompany
    {
        [Key]
        //[Required]
        [Display(Name = "子公司编号")]
        public string CompanyNumber { get; set; }
        [Required]
        [Display(Name = "全称")]
        public string FullName { get; set; }
        [Display(Name = "电话")]
        public string TelNumber { get; set; }
        [Display(Name = "简称")]
        public string ShortName { get; set; }
        [Display(Name = "英文名称")]
        public string EnglishName { get; set; }
        //[Required]
        [Display(Name = "法人代表")]
        public string LegalRepresentative { get; set; }
        //[Required]
        [Display(Name = "成立日期")]
        [DataType(DataType.DateTime)]
        public DateTime EstablishDate { get; set; }
        [Display(Name = "电子邮箱")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+[A-Za-z]{2,4}", ErrorMessage = "电子邮箱是无效的")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "联系地址")]
        public string Address { get; set; }
        [Display(Name = "网址")]
        [Url]
        public string Url { get; set; }
        [Display(Name = "LOGO")]
        public byte[] Logo { get; set; }
        [Display(Name = "备注")]
        public string Remark { get; set; }
    }
    [Table("Departments")]
    public class Department
    {
        [Key]
        //[Required]
        [Display(Name = "部门编号")]
        public string Number { get; set; }

        [Required]
        [Display(Name = "部门名称")]
        public string Name { get; set; }

        [Display(Name = "上级部门")]
        public string ParentDepartmentId { get; set; }
        [Display(Name = "编制人数")]
        public int StaffSize { get; set; }

        [Display(Name = "备注")]
       // [DataType(DataType.MultilineText)]
        public string Remark { get; set; }
    }
   
}