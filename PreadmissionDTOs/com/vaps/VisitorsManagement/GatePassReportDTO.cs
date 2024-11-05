using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VisitorsManagement
{
    public class GatePassReportDTO
    {
        public string GPHS_GatePassNo { get; set; }
        public string GPHS_IDCardNo { get; set; }
        public DateTime? GPHS_DateTime { get; set; }
        public string GPHS_Remarks { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string radiotype { get; set; }
        public Array viewlist { get; set; }
        public string AMST_AdmNo { get; set; }
        public string AMST_FirstName { get; set; }
        public long AMST_MobileNo { get; set; }
        public string AMST_emailId { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public long? HRME_MobileNo { get; set; }
        public long? HRMEMNO_MobileNo { get; set; }
        public string HRME_EmailId { get; set; }
        public string GPHST_GatePassNo { get; set; }
        public string GPHST_IDCardNo { get; set; }
        public DateTime? GPHST_DateTime { get; set; }
        public string GPHST_Remarks { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public Array yarname { get; set; }
        public string ASMAY_Year { get; set; }

        public Array month_list { get; set; }
        public int monthid { get; set; }
        public string monthname { get; set; }
        public string month_id { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public string all1 { get; set; }
        


        //public long MI_Id { get; set; }
        //public string radiotype { get; set; }
        //public Array viewlist { get; set; }
        //public string AMST_AdmNo { get; set; }
        //public string AGPH_PassType { get; set; }
        //public string AGPH_Dategiven { get; set; }
        //public string AGPH_Remark { get; set; }
        //public string AMST_FirstName { get; set; }
        //public long AMST_MobileNo { get; set; }
        //public string AMST_emailId { get; set; }
        //public string HRME_EmployeeCode { get; set; }
        //public string HRME_EmployeeFirstName { get; set; }
        //public long? HRME_MobileNo { get; set; }
        //public string HRME_EmailId { get; set; }
    }
}
