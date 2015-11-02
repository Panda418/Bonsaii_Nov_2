namespace Bonsaii.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StaffParam")]
    public partial class StaffParam
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        [Display(Name="参数值")]
        public string Value { get; set; }

        [Display(Name="参数类型")]
        public int StaffParamTypeId { get; set; }
        [Display(Name = "参数类型")]
        public virtual StaffParamType StaffParamType { get; set; }

        [StringLength(30)]
        public string Extra { get; set; }
    }
}
