using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.HRMS
{
    public class HR_Employee_CommitteeDTO
    {
        public long HRECOM_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HRECOM_CommitteeName { get; set; }
        public string HRECOM_Flg { get; set; }
        public string HRECOM_Role { get; set; }
        public string HRECOM_Document { get; set; }
        public bool HRECOM_ActiveFlg { get; set; }
        public long HRECOM_CreatedBy { get; set; }
        public long HRECOM_UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string retrunMsg { get; set; }
        public string HRECOM_Year { get; set; }
    }
}
