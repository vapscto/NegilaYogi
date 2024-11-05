using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Portals.Staff
{
    public class ClgPortalHRMSDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }    
        public long AMCST_Id { get; set; }
        public long HRME_Id { get; set; }
        public long UserId { get; set; }
        public long HRMED_Id { get; set; }
        public string HRMLY_LeaveYear { get; set; }
        
        public long HRES_Id { get; set; }
        public long hres_id { get; set; }
        public string HRES_Year { get; set; }          
        public string HRML_LeaveName { get; set; }
        public string HRML_LeaveCode { get; set; }
        public string HRML_LeaveDetails { get; set; }
        public bool HRML_LeaveCreditFlg { get; set; }
        public string HRML_LeaveType { get; set; }
        public string monthName { get; set; }
        public decimal? salary { get; set; }
        public string HRMED_Name { get; set; }    
        public decimal? HRESD_Amount { get; set; }
        public string HRES_Month { get; set; }
        public Array yearlist { get; set; }
        public Array currentyear { get; set; }
        public Array TotalEarning { get; set; }
        public Array totalDeduction { get; set; }
        public Array salarylistD { get; set; }
        public Array salarylistE { get; set; }    
        public Array salarylist { get; set; }
        public Array salaryDetailslist { get; set; }
        public Array salaryEarningDlist { get; set; }
    


    }
}
