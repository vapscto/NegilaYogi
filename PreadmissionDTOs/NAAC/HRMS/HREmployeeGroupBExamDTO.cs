using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.HRMS
{
    public class HREmployeeGroupBExamDTO
    {
        public long HREMGBE_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMEGB_Id { get; set; }
        public long HREMGBE_Year { get; set; }
        public string HREMGBE_Remarks { get; set; }
        public string HRMEGB_GroupBExamName { get; set; }
        public string HREMGBE_GPFlg { get; set; }
        public bool HREMGBE_ActiveFlg { get; set; }
        public long HREMGBE_CreatedBy { get; set; }
        public long HREMGBE_UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long HRME_Id { get; set; }
        public string HREMGBE_SubjectName { get; set; }
    }
}
