using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.admission
{
   public class StudentCompliants_DTO
    {
        public long ASCOMP_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public DateTime ASCOMP_Date { get; set; }
        public string ASCOMP_Complaints { get; set; }
        public string ASCOMP_Subject { get; set; }
        public string ASCOMP_FileName { get; set; }
        public string ASCOMP_FilePath { get; set; }
        public long? ASCOMP_ComplaintsBy { get; set; }
        public bool? ASCOMP_ActiveFlg { get; set; }
        public long? ASCOMP_CreatedBy { get; set; }
        public DateTime? ASCOMP_CreatedDate { get; set; }
        public long? ASCOMP_UpdatedBy { get; set; }
        public DateTime? ASCOMP_UpdatedDate { get; set; }
        public Array allacademicyear { get; set; }
        public long ASMAY_Id { get; set; }
        public Array studentlist { get; set; }
        public string AMST_FirstName { get; set; }
        public long UserId { get; set; }
        public string msg { get; set; }
        public Array studentinfolist { get; set; }
        public string AMST_AdmNo { get; set; }
        public Array studentdivlist { get; set; }
        public long AMAY_RollNo { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public Array allstudentdivlist { get; set; }
        public Array alldata1 { get; set; }
        public Array editlist { get; set; }
        public Array viewlist { get; set; }
        public bool returnval { get; set; }
        public string searchfilter { get; set; }
        public DateTime middate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string ReportType { get; set; }
        public Array getreportdetails { get; set; }
        public Array editdata { get; set; }

    }
}
