using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class HR_MRFJOB_DetailsDTO : CommonParamDTO
    {
        public long HRMJD_Id { get; set; }
        public string HRMJD_PositionName { get; set; }
        public string HRMJD_Joblocation { get; set; }
        public long HRMD_Id { get; set; }
        public long HRMP_Id { get; set; }
        public long HRMPT_Id { get; set; }
        public int HRMJD_NoofPosition { get; set; }
        public string HRMJD_Skills { get; set; }
        public string HRMJD_JobDesc { get; set; }
        public long HRMQC_Id { get; set; }
        public decimal HRMJD_ExpFrom { get; set; }
        public decimal HRMJD_ExpTo { get; set; }
        public int HRMJD_Age { get; set; }
        public string HRMJD_Gender { get; set; }
        public string HRMJD_Reason { get; set; }
        public string HRMJD_Selection { get; set; }
        public string HRMJD_Remark { get; set; }
        public long HRMNE_Id { get; set; }
        public bool HRMJD_InternalTalentFlg { get; set; }
        public string HRMJD_Requestsendto { get; set; }
        public string HRMJD_Attachment { get; set; }
        public bool HRMJD_ActiveFlag { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }

}