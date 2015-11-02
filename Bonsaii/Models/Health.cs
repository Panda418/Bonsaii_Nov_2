using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    [Table("Healths")]
    public class Health
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "健康状况")]
        public string HealthCondition { get; set; }
    }
}