namespace Bonsaii.Models.Train
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TrainStarts")]
    public partial class TrainStart
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(4)]
        [Display(Name="单据类别编码")]
        public string BillTypeNumber { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name="单据类别名称")]
        public string BillTypeName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name="单号")]
        public string BillNumber { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name="培训类型")]
        public string TrainType { get; set; }

        [Display(Name="开始时间")]
        public DateTime? StartDate { get; set; }

        [Display(Name="结束时间")]
        public DateTime? EndDate { get; set; }

        [StringLength(50)]
        [Display(Name="培训费用")]
        public string TrainCost { get; set; }

        [StringLength(50)]
        [Display(Name="联系电话")]
        public string TellNumber { get; set; }

        [StringLength(50)]
        [Display(Name="审核状态")]
        public string AuditStatus { get; set; }

        [StringLength(50)]
        [Display(Name="评论")]
        public string Remark { get; set; }
    }
}
