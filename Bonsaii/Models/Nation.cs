using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    [Table("Nations")]
    public class Nation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "民族")]
        public string Nationality { get; set; }
    }
}