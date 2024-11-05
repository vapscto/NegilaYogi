using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class HR_Candidate_InterviewStatusDTO : CommonParamDTO
    {
        public long HRCIS_Id { get; set; }
        public long HRCD_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public string HRCIS_InterviewFeedBack { get; set; }
        public DateTime HRCIS_Datetime { get; set; }
        public string HRCIS_Status { get; set; }
        public bool HRCIS_ActiveFlg { get; set; }
        public DateTime HRCISC_CreatedBy { get; set; }
        public DateTime HRCISC_UpdatedBy { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }

}