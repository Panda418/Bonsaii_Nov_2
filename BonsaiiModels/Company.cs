using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonsaiiModels
{

    [Table("Companies")]
    public class Company
    {
        [Key]
        [Required]
        public string CompanyId { get; set; }
        public string FullName { get; set; }
        public string TelNumber { get; set; }
        public string BusinessLicense { get; set; }
        public string UserName { get; set; }
        public string ParentCompany { get; set; }
        public string ParentCompanyId { get; set; }
        public string ShortName { get; set; }
        public string EnglishName { get; set; }
        public string LegalRepresentative { get; set; }
        public DateTime EstablishDate { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Url { get; set; }
        public FileStream Logo { get; set; }
        public string Remark { get; set; }
        public Boolean IsGroupCompany { get; set; }
        public string GroupCompanyNumber { get; set; }


    }
    [Table("GroupCompanies")]
    public class GroupCompany
    {
        [Key]
        [Required]
        public string CompanyNumber { get; set; }
        public string FullName { get; set; }
        public string TelNumber { get; set; }

        public string ShortName { get; set; }
        public string EnglishName { get; set; }
        public string LegalRepresentative { get; set; }
        public DateTime EstablishDate { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Url { get; set; }
        public FileStream Logo { get; set; }
        public string Remark { get; set; }
    }
    public class Test
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
