using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Training
{
   public class External_TrainingDTO
    {
        public long HREXTTRN_Id { get; set; }
        public long MI_Id { get; set; }
        public long Userid { get; set; }
        public long roleid { get; set; }
        public long HRME_Id { get; set; }
        public long HRMETRCEN_Id { get; set; }
        public long HRMETRTY_Id { get; set; }
        public long hrmeid { get; set; }
        public DateTime? HREXTTRN_StartDate { get; set; }
        public DateTime? HREXTTRN_EndDate { get; set; }
        public string HREXTTRN_StartTime { get; set; }
        public string HREXTTRN_EndTime { get; set; }
        public decimal HREXTTRN_TotalHrs { get; set; }
        public string HREXTTRN_ApprovedFlg { get; set; }
        public string HRMEM_EmailId { get; set; }
        public string ISMMCLT_ClientName { get; set; }
        public bool HREXTTRN_ActiveFlag { get; set; }
        public DateTime HREXTTRN_CreatedDate { get; set; }
        public DateTime HREXTTRN_UpdatedDate { get; set; }
        public long HREXTTRN_CreatedBy { get; set; }
        public long HREXTTRN_UpdatedBy { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
        public Array getloaddetails { get; set; }
        public Array trainingcentername { get; set; }
        public Array externaltrainingtype { get; set; }
        public Array trainingdetails { get; set; }
        public Array participates_Employee_list { get; set; }
        public Array employeename { get; set; }
        public Array editdata { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRMETRCEN_TrainingCenterName { get; set; }
        public string HRMETRTY_ExternalTrainingType { get; set; }
        public string HREXTTRN_TrainingTopic { get; set; }
        
        public emplyee1[] emplyee { get; set; }
        public bool vapstraining { get; set; }
        public class emplyee1
        {
            public long HRME_Id { get; set; }
        }

        public filedto[] FilePath_Array { get; set; }
    }
    public class filedto
    {
        public string FOINF_FileName { get; set; }
        public string HREXTTRN_TrainingTopic { get; set; }
        public string HREXTTRN_CertificateFilePath { get; set; }
        public string HREXTTRN_CertificateFileName { get; set; }
    }
}

