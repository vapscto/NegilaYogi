using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Placement
{
    [Table("PL_CI_Schedule_Company_JobTitle_Criteria")]
    public  class PL_CI_Schedule_Company_JobTitle_CriteriaDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PLCISCHCOMJTCR_Id { get; set; }
        public long PLCISCHCOMJT_Id { get; set; }
        public long AMCO_Id { get; set; }
        public decimal PLCISCHCOMJTCR_CutOfMark { get; set; }
        public string PLCISCHCOMJTCR_OtherDetails { get; set; }
        public bool PLCISCHCOMJTCR_ActiveFlag { get; set; }     
        public DateTime? PLCISCHCOMJTCR_UpdatedDate { get; set; }
        public DateTime? PLCISCHCOMJTCR_CreatedDate { get; set; }
        public long PLCISCHCOMJTCR_CreatedBy { get; set; }
        public long PLCISCHCOMJTCR_UpdatedBy { get; set; }

    }
}
