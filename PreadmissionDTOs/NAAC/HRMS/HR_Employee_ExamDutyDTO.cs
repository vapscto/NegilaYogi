using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.HRMS
{
    public class HR_Employee_ExamDutyDTO
    {
        public long HREEXDT_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HREEXDT_ExamDutyType { get; set; }
        public string HREEXDT_ExaminerType { get; set; }
        public string HREEXDT_CollUniName { get; set; }
        public bool HREEXDT_ActiveFlg { get; set; }
        public long HREEXDT_CreatedBy { get; set; }
        public long HREEXDT_UpdatedBy { get; set; }
        public string retrunMsg { get; set; }
    }
}
