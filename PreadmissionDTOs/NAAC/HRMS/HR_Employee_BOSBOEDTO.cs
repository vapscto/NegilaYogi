using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.HRMS
{
    public class HR_Employee_BOSBOEDTO
    {
        public long HREBOS_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HREBOS_Subject { get; set; }
        public string HREBOS_BOSBOEFlg { get; set; }
        public string HREBOS_UnvCollegeFlg { get; set; }
        public DateTime HREBOS_FromToDate { get; set; }
        public DateTime HREBOS_ToDate { get; set; }
        public string HREBOS_Document { get; set; }
        public bool HREBOS_ActiveFlg { get; set; }
        public long HREBOS_CreatedBy { get; set; }
        public long HREBOS_UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string retrunMsg { get; set; }
        public string HREBOS_Year { get; set; }
        public string HREBOS_Role { get; set; }
        public bool? HREBOS_ApprovedFlg { get; set; }
        public string HREBOS_Remarks { get; set; }
        public string HREBOS_StatusFlg { get; set; }
    }
}
