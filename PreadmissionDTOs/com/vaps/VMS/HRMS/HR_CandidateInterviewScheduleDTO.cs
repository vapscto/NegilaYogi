using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class HR_CandidateInterviewScheduleDTO : CommonParamDTO
    {
        public long HRCISC_Id { get; set; }
        public long HRCD_Id { get; set; }
        public long MI_Id { get; set; }
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
        public string HRCISC_InterviewDate { get; set; }

        public string retrunMsg { get; set; }
        public string HRCD_FullName { get; set; }
        public string HRCD_FirstName { get; set; }
        public string HRCD_MiddleName { get; set; }
        public string HRCD_LastName { get; set; }
        public string HRCD_Photo { get; set; }
        public string HRCD_Resume { get; set; }
        public long? HRCMG_Id { get; set; }
        public Array VMSEditValue { get; set; }
        public Array gradelist  { get; set; }
        public Array VMSCandidateInterviewList { get; set; }
        public Array CandidateDetailsList { get; set; }
        public Array InterviewerList { get; set; }
        public Array calenderlist { get; set; }
        public Array upcomingintvw { get; set; }
        public Array inprogressintvw { get; set; }
        public Array completedintvw { get; set; }

        //REPORT
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public string rdotype { get; set; }
        //REPORT

        public long Id { get; set; }
        public long HRME_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public long IVRMUL_Id { get; set; }
        public DateTime HRCIS_Datetime { get; set; }
        public string HRCIS_InterviewFeedBack { get; set; }
        public string HRCIS_Status { get; set; }
        public long UserId { get; set; }
        public string HRCIS_CandidateStatus { get; set; }
    }

}