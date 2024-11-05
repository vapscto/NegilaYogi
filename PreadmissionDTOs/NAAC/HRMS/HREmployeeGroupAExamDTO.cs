using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.HRMS
{
    public class HREmployeeGroupAExamDTO
    {
        public long HREMGAE_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long HRMEGA_Id { get; set; }
        public long HREMGAE_Year { get; set; }
        public string HREMGAE_GPFlg { get; set; }
        public string HREMGAE_Marks { get; set; }
        public bool HREMGAE_ActiveFlg { get; set; }
        public long HREMGAE_CreatedBy { get; set; }
        public long HREMGAE_UpdatedBy { get; set; }
        public string HRMEGA_GroupAExamName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string HREMGAE_SubjectName { get; set; }
    }
}
