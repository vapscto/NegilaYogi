using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class HR_MRFRequisitionDTO : CommonParamDTO
    {
        public long HRMRFR_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMP_Id { get; set; }
        public string HRMP_Desc { get; set;}
        public string HRMP_Skills { get; set;}
        public long HRMLO_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long HRMPR_Id { get; set; }
        public long HRMPT_Id { get; set; }
        public long HRMRFR_MRFNO { get; set; }
        public long HRMC_Id { get; set; }
        public long HRMRFR_NoofPosition { get; set; }
        public string HRMRFR_Skills { get; set; }
        public string HRMRFR_JobDesc { get; set; }
        public decimal HRMRFR_ExpFrom { get; set; }
        public decimal HRMRFR_ExpTo { get; set; }
        public long HRMRFR_Age { get; set; }
        public long IVRMMG_Id { get; set; }
        public string HRMRFR_Reason { get; set; }
        public bool HRMRFR_WrittenTestFlg { get; set; }
        public bool HRMRFR_OnlineTestFlg { get; set; }
        public bool HRMRFR_TechnicalInterviewFlg { get; set; }
        public string HRMRFR_Remark { get; set; }
        public string HRMRFR_Attachment { get; set; }
        public bool HRMRFR_ActiveFlag { get; set; }
        public string HRMRFR_Status { get; set; }
        public long HRMRFR_CreatedBy { get; set; }
        public long HRMRFR_UpdatedBy { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
        public bool HRMRFR_ManagerFlag { get; set; }
        public bool HRMRFR_HRFlag { get; set; }
        public bool? HRMRFR_MDFlag { get; set; }
        public decimal? HRMRFR_PayScaleFrom { get; set; }
        public decimal? HRMRFR_PayScaleTo { get; set; }
        public DateTime? HRMRFR_PositionFilled { get; set; }
        public string HRMRFR_HRComment { get; set; }
        public string HRMRFR_MDComment { get; set; }
        public string UserName { get; set; }

        public string retrunMsg { get; set; }
        public string HRMP_Position { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMPT_Name { get; set; }
        public string HRMLO_LocationName { get; set; }
        public long userid { get; set; }
        public string HRMP_Name { get; set; }
        public string HRMC_QulaificationName { get; set; }
        public string IVRMMG_GenderName { get; set; }
        public string HRMRFR_JobLocation { get; set; }
        public long ISMMCLT_Id { get; set; }
        public string ISMMCLT_ClientName { get; set; }
        public long HRME_Id { get; set; }


        public Array VMSMRFList { get; set; }
        public Array MasterPositionList { get; set; }
        public Array MasterDepartmentList { get; set; }
        public Array MasterPriorityList { get; set; }
        public Array MasterPosTypeList { get; set; }
        public Array MasterQualification { get; set; }
        public Array MasterLocation { get; set; }
        public Array MasterGender { get; set; }
        public Array VMSEditValue { get; set; }
        public Array clientlist { get; set; }
    }

}