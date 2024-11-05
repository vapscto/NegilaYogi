using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.HRMS
{
    public class NAACACCommitteeDTO
    {
        public long NCACCOMM_Id { get; set; }
        public long MI_Id { get; set; }
        public string NCACCOMM_CommitteeName { get; set; }
        public string NCACCOMM_Flg { get; set; }
        public long NCACCOMM_Year { get; set; }
        public string NCACCOMM_FileName { get; set; }
        public string NCACCOMM_FilePath { get; set; }
        public bool NCACCOMM_ActiveFlg { get; set; }
        public long NCACCOMM_CreatedBy { get; set; }
        public long NCACCOMM_UpdatedBy { get; set; }
        public DateTime? NCACCOMM_CreatedDate { get; set; }
        public DateTime? NCACCOMM_UpdatedDate { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }
        public Array commetteeList { get; set; }
    }
}
