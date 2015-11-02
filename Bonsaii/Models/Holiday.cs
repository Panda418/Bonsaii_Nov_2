using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    [Table("Holidaies")]
    public class Holiday
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "假日名称")]
        public string JieJiaName { get; set; }
        [Display(Name = "开始时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime BeginTime { get; set; }
        [Display(Name = "结束时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime EndTime { get; set; }
    }
}