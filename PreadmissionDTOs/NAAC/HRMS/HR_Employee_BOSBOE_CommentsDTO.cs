using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.HRMS
{
    public class HR_Employee_BOSBOE_CommentsDTO
    {
        public long NCHRBOSC_Id { get; set; }
        public long HREBOS_Id { get; set; }
        public string NCHRBOSC_Remarks { get; set; }
        public long NCHRBOSC_RemarksBy { get; set; }
        public string RemarkPersonname { get; set; }
        public string NCHRBOSC_StatusFlg { get; set; }
        public DateTime? NCHRBOSC_CreatedDate { get; set; }
        public DateTime? NCHRBOSC_UpdatedDate { get; set; }
        public bool NCHRBOSC_ActiveFlag { get; set; }
        public long NCHRBOSC_CreatedBy { get; set; }
        public long NCHRBOSC_UpdatedBy { get; set; }
    }
}
