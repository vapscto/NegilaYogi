using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
  [Table("HR_Employee_Increment")]
  public class HR_Employee_Increment 
    {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HREIC_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime HREIC_LastIncrementDate { get; set; }
        public DateTime HREIC_IncrementDueDate { get; set; }
        public DateTime HREIC_IncrementDate { get; set; }
        public DateTime? HREIC_NextIncrementGivenDate { get; set; }
        public bool? HREIC_ArrearApplicableFlg { get; set; }
        public bool? HREIC_ArrearGivenFlg { get; set; }
        public long HREIC_ArrearMonths { get; set; }
        public bool HREIC_ActiveFlag { get; set; }
        public long HREIC_CreatedBy { get; set; }
        public long HREIC_UpdatedBy { get; set; }
        public DateTime HREIC_CreatedDate { get; set; }
        public DateTime HREIC_UpdatedDate { get; set; }
    }
}
