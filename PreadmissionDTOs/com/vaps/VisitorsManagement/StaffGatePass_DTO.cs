using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VisitorsManagement
{
    public class StaffGatePass_DTO : CommonParamDTO
    {
        public Master_NumberingDTO transnumbconfigurationsettingsss { get;set;}
        public long GPHST_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string GPHST_GatePassNo { get; set; }
        public string GPHST_IDCardNo { get; set; }
        public DateTime? GPHST_DateTime { get; set; }
        public string GPHST_Remarks { get; set; }
        public bool GPHST_ActiveFlg { get; set; }
        public long? GPHST_CreatedBy { get; set; }
        public long? GPHST_UpdatedBy { get; set; }
        public long ASMAY_Id { get; set; }
        public long UserId { get; set; }
        public Array yearlist { get; set; }
        public Array filldepartment { get; set; }
        public Array alldata { get; set; }
        public Array emplist { get; set; }
        public Array filldesignation { get; set; }
        public Array editlist { get; set; }
        public Array currentstaffdata { get; set; }
        public Array institution { get; set; }
        public bool returnval { get; set; }
        public bool dulicate { get; set; }
        public string empname { get; set; }
        public string trans_id { get; set; }
        public long? HRMD_Id { get; set; }
        public long? HRMDES_Id { get; set; }
        public long? HRMEMNO_MobileNo { get; set; }
        public long? HRME_MobileNo { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public string GPHST_InTime { get; set; }
        public string GPHST_OutTime { get; set; }
        public string HRME_Photo { get; set; }
    }
}