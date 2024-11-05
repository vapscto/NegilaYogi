using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class HR_JobDetailsDTO : CommonParamDTO
    {
        public long HRJD_Id { get; set; }
        public long HRMJ_Id { get; set; }
        public long HRJA_Id { get; set; }
        public string HRMJ_JobTiTle { get; set; }
        public string HRMJ_KeySkills { get; set; }
        public string HRMJ_JobLocation { get; set; }
        public int HRMJ_NoofPosition { get; set; }
        public string HRMJ_JobRole { get; set; }
        public long MI_Id { get; set; }
        public decimal HRMJ_ExperienceFrom { get; set; }
        public decimal HRMJ_ExperienceTo { get; set; }
        public string HRMJ_CTC { get; set; }

        public string HRMJ_JobType { get; set; }
        public string HRMJ_Qualification { get; set; }
        public string HRMJ_JobDesc { get; set; }
        public string HRMJ_ComapanyInfo { get; set; }
        public string HRMJ_ContactEmail { get; set; }
        public long HRMJ_ContactNumber { get; set; }
        public string HRMJ_ContactPerson { get; set; }
        public bool HRMJ_PublishOnVaps { get; set; }
        public bool HRMJ_ActiveFlg { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }

        public string retrunMsg { get; set; }
        public Array VMSJobList { get; set; }
        public Array MasterJobList { get; set; }
    }

}