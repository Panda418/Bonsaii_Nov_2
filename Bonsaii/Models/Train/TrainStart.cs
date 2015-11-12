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
        [Display(Name="����������")]
        public string BillTypeNumber { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name="�����������")]
        public string BillTypeName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name="����")]
        public string BillNumber { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name="��ѵ����")]
        public string TrainType { get; set; }

        [Display(Name="��ʼʱ��")]
        public DateTime? StartDate { get; set; }

        [Display(Name="����ʱ��")]
        public DateTime? EndDate { get; set; }

        [StringLength(50)]
        [Display(Name="��ѵ����")]
        public string TrainCost { get; set; }

        [StringLength(50)]
        [Display(Name="��ϵ�绰")]
        public string TellNumber { get; set; }

        [StringLength(50)]
        [Display(Name="���״̬")]
        public string AuditStatus { get; set; }

        [StringLength(50)]
        [Display(Name="����")]
        public string Remark { get; set; }
    }
}
