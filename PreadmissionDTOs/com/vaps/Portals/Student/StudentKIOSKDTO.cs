using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Student
{
    public class StudentKIOSKDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMST_Id { get; set; }

        public Array studetailslist { get; set; }
        public Array academicyearFeedata { get; set; }
        public Array academicyearAttendancedata { get; set; }
    }

    public class StudentKioskExamTopperDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long EME_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_AdmNo { get; set; }
        public decimal ESTMP_TotalMaxMarks { get; set; }
        public decimal ESTMP_TotalObtMarks { get; set; }
        public decimal ESTMP_Percentage { get; set; }
        public int Class_Rnk { get; set; }
        public string ELP_Flg { get; set; }
        public Array classranklist { get; set; }
    }

    public class StudentKioskCOEDTO
    {
        public long ASMCL_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMST_Id { get; set; }
        public long MI_Id { get; set; }
        public long month { get; set; }
        public string ASMCL_ClassName { get; set; }
        public long ASMS_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public string COEME_EventName { get; set; }
        public string COEME_EventDesc { get; set; }
        public DateTime? COEE_EStartDate { get; set; }
        public DateTime? COEE_EEndDate { get; set; }

        public Array coereportlist { get; set; }
    }

    public class StudentKioskSubjectDTO
    {
        public long AMST_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMAY_Year { get; set; }
        public string ISMS_SubjectName { get; set; }
        public int ASMAY_Order { get; set; }

        public Array stuyearlist { get; set; }
        public Array subjectlist { get; set; }
    }

    public class StudentKioskEXAMDTO
    {
        public long ASMCL_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMST_Id { get; set; }
        public long MI_Id { get; set; }
        public string EME_Id { get; set; }
        public string ISMS_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public long ASMS_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public string ASMAY_Year { get; set; }
        public string Type { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public Array examReportList { get; set; }
        public Array subjectlist { get; set; }
        public Array examlist { get; set; }
    }

    public class StudentKioskFEEDTO
    {
        public long AMST_Id { get; set; }
        public long MI_Id { get; set; }
        public long FSS_CurrentYrCharges { get; set; }
        public long FSS_ToBePaid { get; set; }
        public long FSS_PaidAmount { get; set; }
        public long FSS_ConcessionAmount { get; set; }
        public string FTI_Name { get; set; }
        public string FMH_FeeName { get; set; }
        public Array studentfeedetails { get; set; }
        public Array feeAnalysisList { get; set; }
    }

    public class StudentKioskSPORTSDTO
    {
        public long MI_Id { get; set; }
        public string eventName { get; set; }
        public string studentName { get; set; }
        public string sportsName { get; set; }
        public string studentPhotoPath { get; set; }
        public int SPCCESTR_Place { get; set; }
        public Array winnerList { get; set; }
    }

    public class StudentKioskBIRTHDAYDTO
    {
        public DateTime? Fromdate { get; set; }
        public DateTime? Todate { get; set; }
        public DateTime? amst_dob { get; set; }
        public DateTime? HRME_DOB { get; set; }
        public int months { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long HRME_Id { get; set; }
        public long AMST_MobileNo { get; set; }
        public long HRME_MobileNo { get; set; }
        public string all1 { get; set; }
        public string rdbbutton { get; set; }
        public string studentName { get; set; }
        public string employeeName { get; set; }
        public string AMST_emailId { get; set; }
        public string HRME_EmailId { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string PhotoPath { get; set; }
        public int count { get; set; }
        public Array staffDetails { get; set; }
        public Array studentlist { get; set; }
        public Array staffList { get; set; }
    }

    public class StudentKioskHomeWorkDTO
    {
        public DateTime? IHW_Date { get; set; }
        public long ASMAY_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long IHW_AssignmentNo { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string IHW_Assignment { get; set; }
        public string IHW_Attachment { get; set; }
        public string IHW_FilePath { get; set; }
        public string IHW_Topic { get; set; }
        public string ISMS_SubjectName { get; set; }
        public Array homeworklist { get; set; }
    }

    public class StudentKioskNoticeDTO
    {
        public long ASMAY_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMS_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public Array homeworklist { get; set; }
        public Array noticelist { get; set; }
        public Array studetailslist { get; set; }
        public string message { get; set; }
    }

    public class StudentKioskTimeTableDTO
    {
        public long ASMAY_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMS_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public Array getStudentTT { get; set; }
    }
}