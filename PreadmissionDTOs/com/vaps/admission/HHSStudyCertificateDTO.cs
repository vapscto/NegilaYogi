using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class HHSStudyCertificateDTO
    {
        public long MI_Id { get; set; }
        public long userId { get; set; }
        public string AMST_FirstName { get; set; }
        public long AMST_Id { get; set; }
        public string AMST_SOL { get; set; }
        public Array fillstudlist { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public string AMST_AdmNo { get; set; }
        public Array AllAcademicYear { get; set; }
        public Array adm_m_student { get; set; }
        public string IMC_CasteName { get; set; }
        public long ASMAY_Id { get; set; }
        public long? ASMCL_Id { get; set; }
        public long ASMC_Id { get; set; }
        public Array studentlist { get; set; }
        public Array studentlist1 { get; set; }
        public Array academicYearOnLoad { get; set; }
        public Array StudentList { get; set; }
        public Array academicyearforreadmit { get; set; }
        public Array MasterCompany { get; set; }
        public string companyname { get; set; }
        public Array academicList1 { get; set; }
        public Array allsectionlist { get; set; }
        public Array allclasslist { get; set; }
        public string searchfilter { get; set; }
        public int count { get; set; }
        public string message { get; set; }
        public string photopath { get; set; }
        public string allorindid { get; set; }
        public string address { get; set; }
        public string milogo { get; set; }
        public Array studentreportcount { get; set; }
        public Array leftstudentdetails { get; set; }
        public Array GetReportTypes { get; set; }
        public string save_flag { get; set; }
        public string Report_Name { get; set; }
        public string Report_Type { get; set; }
        public string ReportType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public Array GetReportDetails { get; set; }
    }
}
