using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.HRMS
{
    public class NAACACCommitteeMembersDTO
    {
        public long NCACCOMMM_Id { get; set; }
        public long NCACCOMM_Id { get; set; }
        public long HRME_Id { get; set; }
        public string NCACCOMMM_MemberName { get; set; }
        public string NCACCOMMM_MemberDetails { get; set; }
        public long NCACCOMMM_MemberPhoneNo { get; set; }
        public string NCACCOMMM_MemberEmailId { get; set; }
        public string NCACCOMMM_Role { get; set; }
        public string NCACCOMMM_FileName { get; set; }
        public string NCACCOMMM_FilePath { get; set; }
        public bool NCACCOMMM_ActiveFlg { get; set; }
        public long NCACCOMMM_CreatedBy { get; set; }
        public long NCACCOMMM_UpdatedBy { get; set; }
        public DateTime? NCACCOMMM_CreatedDate { get; set; }
        public DateTime? NCACCOMMM_UpdatedDate { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }
        public Array commetteeList { get; set; }
        public Array empdropdownlist { get; set; }
        public Array committeedropdownlist { get; set; }
        public long MI_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string NCACCOMM_CommitteeName { get; set; }
    }
}
