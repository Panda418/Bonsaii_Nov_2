namespace Bonsaii.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ReserveFields")]
    public partial class ReserveField
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [StringLength(50)]
        [Display(Name="表名")]
        public string TableName { get; set; }

        [StringLength(50)]
        [Display(Name="字段名")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "请输入正确的英文名称")]
        public string FieldName { get; set; }

        [StringLength(50)]
        [Display(Name="描述")]
        public string Description { get; set; }

        [StringLength(50)]
        [Display(Name="状态")]
        public string Status { get; set; }
    }
}
