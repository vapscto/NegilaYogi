using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("HR_Candidate_InterviewStatus")]
    public class HR_Candidate_InterviewStatusDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRCIS_Id { get; set; }
        public long HRCD_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public string HRCIS_InterviewFeedBack { get; set; }
        public DateTime HRCIS_Datetime { get; set; }
        public string HRCIS_Status { get; set; }
        public bool HRCIS_ActiveFlg { get; set; }
        public long HRCISC_CreatedBy { get; set; }
        public long HRCISC_UpdatedBy { get; set; }
        public string HRCIS_CandidateStatus { get; set; }
        public long? HRCMG_Id  { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }

}