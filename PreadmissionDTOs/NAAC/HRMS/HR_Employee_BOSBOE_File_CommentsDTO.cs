using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.HRMS
{
    public class HR_Employee_BOSBOE_File_CommentsDTO
    {
        public long NCHRBOSFC_Id { get; set; }
        public long NCHREBOSF_Id { get; set; }
        public string NCHRBOSFC_Remarks { get; set; }
        public long NCHRBOSFC_RemarksBy { get; set; }
        public bool NCHRBOSFC_ActiveFlag { get; set; }
        public long NCHRBOSFC_CreatedBy { get; set; }
        public long NCHRBOSFC_UpdatedBy { get; set; }
        public DateTime? NCHRBOSFC_CreatedDate { get; set; }
        public DateTime? NCHRBOSFC_UpdatedDate { get; set; }
        public string NCHRBOSFC_StatusFlg { get; set; }
        public string RemarkPersonname { get; set; }
    }
}
