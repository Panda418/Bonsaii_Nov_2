using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    [Table("Nationalities")]
    public class Nationality
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "序号")]
        public int Sort { get; set; }
        [Required]
        [Display(Name = "国籍")]
        public string Nation { get; set; }
    }
}