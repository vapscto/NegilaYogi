using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.HealthManagement
{
    public class HM_Illness_StudentEntryDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long HMTILL_Id { get; set; }
        public long HMMILL_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public DateTime? HMTILL_Date { get; set; }
        public DateTime? HMTILL_CreatedDate { get; set; }
        public bool HMTILL_ActiveFlg { get; set; }
        public bool ReturnValue { get; set; }
        public bool smschecked { get; set; }
        public bool emailchecked { get; set; }
        public bool whatsappchecked { get; set; }
        public string Message { get; set; }
        public string Searchfilter { get; set; }
        public string StudentName { get; set; }
        public string StudentName_Edit { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public string YearName { get; set; }
        public string AdmissionNo { get; set; }       
        public string HMMILL_IllnessName { get; set; }
        public Array GetMasterAcademicYearList { get; set; }
        public Array GetMasterClassList { get; set; }
        public Array GetMasterSectionList { get; set; }
        public Array GetMasterStudentList { get; set; }
        public Array GetMasterIllnessList { get; set; }
        public Array GetTransactionIllnessList { get; set; }
        public Array GetStudentYearData { get; set; }
        public Array GetEditStudentIllnessData { get; set; }
        public Array GetEditStudentData { get; set; }
        public Array GetEditIllnessData { get; set; }

        // Student Illness Report
        public Array GetReportAcademicYearList { get; set; }
        public Array GetReportStudentList { get; set; }
        public Array GetReportDataList { get; set; }
        public Array GetMasterInstitutionDetails { get; set; }
        public string ReportType { get; set; }
    }
}