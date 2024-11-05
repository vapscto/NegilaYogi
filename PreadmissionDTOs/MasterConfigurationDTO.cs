using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class MasterConfigurationDTO : CommonParamDTO
    {
        public long ISPAC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long MO_Id { get; set; }
        public int ISPAC_EnquiryApplFlag { get; set; }
        public int ISPAC_ApplIssueFlag { get; set; }

        public int ISPAC_RegFeeFlag { get; set; }
        public int ISPAC_EnqSMSFlag { get; set; }
        public int ISPAC_EnqMailFlag { get; set; }
        public int ISPAC_EnqMailBackground { get; set; }
        public int ISPAC_ApplFeeFlag { get; set; }
        public int ISPAC_ApplnDownladFlag { get; set; }
        public int ISPAC_ApplSMSFlag { get; set; }
        public int ISPAC_ApplMailFlag { get; set; }
        public int ISPAC_ApplMailBackground { get; set; }
        public int ISPAC_DOBMaxAgeFlag { get; set; }
        public int ISPAC_DOBMinAgeFlag { get; set; }
        public int ISPAC_ApplCutOffDateFlag { get; set; }
        public int ISPAC_ApplNoGenFlag { get; set; }
        public int ISPAC_DefaultStatusFlag { get; set; }
        public int ISPAC_OralTestBy { get; set; }


        public int ISPAC_EnquiryNoGenFlag { get; set; }
        public int ISPAC_RgNoGenFlag { get; set; }
        public int ISPAC_RegSMSFlag { get; set; }
        public int ISPAC_RegMailFlag { get; set; }
        public int ISPAC_RegMailBackground { get; set; }
        public int ISPAC_AdmCategoryFlag { get; set; }
        public int ISPAC_WrittenTestApplFlag { get; set; }
        public int ISPAC_MarksEntry { get; set; }
        public int ISPAC_OralTestApplFlag { get; set; }
        public int ISPAC_SeatBlockFlag { get; set; }
        public int ISPAC_FatherAliveFlag { get; set; }
        public int ISPAC_MotherAliveFlag { get; set; }
        public int ISPAC_MaritalStatusFlag { get; set; }
        public int ISPAC_FeePaymentFlag { get; set; }
        public int ISPAC_AdmissionTransfer { get; set; }
        public int ISPAC_EnableSiblingsLink { get; set; }
        public int ISPAC_SibblingConcessionFlag { get; set; }
        public int ISPAC_ParentConcessionFlag { get; set; }
        public int ISPAC_ECSFlag { get; set; }
        public int ISPAC_HostelFlag { get; set; }
        public int ISPAC_TransaportFlag { get; set; }
        public int ISPAC_GymFlag { get; set; }
        public int ISPAC_EnquiryNo_ApplNo_RegNo { get; set; }
        public Array mstConfigList { get; set; }
        public int ISPAC_Healthapp { get; set; }
        public Array AcademicList { get; set; }
        // To hold the message on 9-11-2016
        public string message { get; set; }
        public decimal ISPAC_OralByMax_Marks { get; set; }

        public int ISPAC_ProspectusFlag { get; set; }

        public int ISPAC_Transfer_Settings_Flag { get; set; }

        public int ISPAC_Transfer_Settings_after_payment_Flag { get; set; }

        public string MO_Name { get; set; }
        public string MI_Name { get; set; }
        public string ASMAY_Year { get; set; }

        public int AMC_Id { get; set; }
        public int ISPAC_ProsptFeeApp { get; set; }


        public int ISPAC_CommonScheduleFlag { get; set; }
        public int ISPAC_WrittenTestSchApplFlag { get; set; }
        public int ISPAC_OralTestSchApplFlag { get; set; }

        public int ISPAC_OfflineFee_Flag { get; set; }


        public int ISPAC_OralMarksFlag { get; set; }

        // To hold the message on 

        public int ISPAC_NoofApplications { get; set; }

        public string MI_SchoolCollegeFlag { get; set; }

        public bool? ISPAC_ApplNoIncrementFlg { get; set; }
        public int? ISPAC_ApplNoIncrementBy { get; set; }
    }
}
