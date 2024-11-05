using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Training
{
    public class External_Training_ApprovalDTO
    {
        public long HREXTTRNAPP_Id { get; set; }
        public long HREXTTRN_Id { get; set; }
        public long HRME_Id { get; set; }
        public long MI_Id { get; set; }
        public long Userid { get; set; }
        public long roleid { get; set; }
        public long LoginId { get; set; }
        public long hrmeid { get; set; }
        public string EmplYoeeName { get; set; }
        public string HREXTTRNAPP_ApproverRemarks { get; set; }
        public string HREXTTRN_TrainingTopic { get; set; }
        public decimal? HREXTTRNAPP_ApprovedHours { get; set; }
        public decimal HREXTTRN_TotalHrs { get; set; }
        public string HREXTTRN_ApprovedFlg { get; set; }
        public string HREXTTRN_ApprovedFlg1 { get; set; }

        public decimal HREXTTRNAPP_ApprovedHrs { get; set; }
        public bool HREXTTRNAPP_ActiveFlag { get; set; }     
        public DateTime HREXTTRNAPP_CreatedDate { get; set; }
        public DateTime HREXTTRNAPP_UpdatedDate { get; set; }
        public long HREXTTRNAPP_CreatedBy { get; set; }
        public long HREXTTRNAPP_UpdatedBy { get; set; }
        public string returnval { get; set; }
        public string message { get; set; }
        public Array getloaddetails { get; set; }
        public multiapprovalT[] multiapproval { get; set; }
        public Array loadgrid { get; set; }
        public Array Totalapprovaleval { get; set; }
        public Array trainingdetails { get; set; }
        public bool approveflag { get; set; }
        public string HRMETRCEN_TrainingCenterName { get; set; }
        public string HRMETRTY_ExternalTrainingType { get; set; }
        public string HREXTTRN_CertificateFilePath { get; set; }
        public DateTime? HREXTTRN_StartDate { get; set; }
        public DateTime? HREXTTRN_EndDate { get; set; }
        public string HREXTTRN_StartTime { get; set; }
        public string HREXTTRN_EndTime { get; set; }
        public string HREXTTRNAPP_ApprovalFlg { get; set; }
    }
    public class multiapprovalT
    {
        public string HREXTTRNAPP_ApproverRemarks { get; set; }
        public long HREXTTRN_Id { get; set; }
        public string HREXTTRN_TrainingTopic { get; set; }
        public decimal? HREXTTRNAPP_ApprovedHrs { get; set; }
        //public bool HREXTTRN_ApprovedFlg { get; set; }

        public string hrexttrn_flg { get; set; }
    }
}
