using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.HRMS
{
   public class HR_Employee_Awards_DTO
    {
        public long HREAW_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HREAW_AwardName { get; set; }
        public long HREAW_AwardYear { get; set; }
        public decimal? HREAW_IncentiveAmount { get; set; }
        public string HREAW_FileName { get; set; }
        public string HREAW_FilePath { get; set; }
        public bool HREAW_ActiveFlg { get; set; }
        public long HREAW_CreatedBy { get; set; }
        public long HREAW_UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string HREAW_LevelAwards { get; set; }
        public string HREAW_AgencyName { get; set; }

        public int? HRME_EmployeeOrder { get; set; }
        public long UserId { get; set; }
        public long? HRMDES_Id { get; set; }
        public long? HRMD_Id { get; set; }
        public string empname { get; set; }
        public string empcode { get; set; }
        public string mobileNo { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMDES_DesignationName { get; set; }

        public Array filldepartment { get; set; }
        public Array filldesignation { get; set; }
        public Array emplist { get; set; }
        public Array alldata { get; set; }
        public Array editlist { get; set; }
        public Array yearlist { get; set; }

        public bool returnval { get; set; }
        public bool duplicate { get; set; }

        public string ASMAY_Year { get; set; }
        public long ASMAY_Id { get; set; }
        public Array yearlist_edit { get; set; }

    }
}
