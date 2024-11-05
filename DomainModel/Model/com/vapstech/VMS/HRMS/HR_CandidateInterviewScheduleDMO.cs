using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("HR_Candidate_InterviewSchedule")]
    public class HR_CandidateInterviewScheduleDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRCISC_Id { get; set; }
        public long HRCD_Id { get; set; }
        public string HRCISC_InterviewRounds { get; set; }
        public DateTime HRCISC_InterviewDateTime { get; set; }
        public string HRCISC_InterviewVenue { get; set; }
        public bool HRCISC_ActiveFlg { get; set; }
        public long HRCISC_CreatedBy { get; set; }
        public long HRCISC_UpdatedBy { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
        public long HRCISC_Interviewer { get; set; }
        public string HRCISC_Status { get; set; }
        public bool HRCISC_NotifyEmail { get; set; }
        public bool HRCISC_NotifySMS { get; set; }
    }

}