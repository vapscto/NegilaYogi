using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Placement
{
    [Table("PL_CI_Schedule_Company_JobTitle")]
    public class PL_CI_Schedule_Company_JobTitleDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PLCISCHCOMJT_Id { get; set; }
        public long PLCISCHCOM_Id { get; set; }
        public string PLCISCHCOMJT_JobTitle { get; set; }
        public string PLCISCHCOMJT_QulaificationCriteria { get; set; }
        public string PLCISCHCOMJT_OtherDetails { get; set; }
        public bool PLCISCHCOMJT_ActiveFlag { get; set; }
        public DateTime? PLCISCHCOMJT_CreatedDate { get; set; }
        public DateTime? PLCISCHCOMJT_UpdatedDate { get; set; }
        public long PLCISCHCOMJT_CreatedBy { get; set; }
        public long PLCISCHCOMJT_UpdatedBy { get; set; }
        public long PLCISCHCOMJT_NoOfInterviewRounds { get; set; }
    }
}
