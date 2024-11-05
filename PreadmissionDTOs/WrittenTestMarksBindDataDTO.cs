using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class WrittenTestMarksBindDataDTO : CommonParamDTO
    {
        public Array fillyear { get; set; }
        public Array studentDetails { get; set; }
        public int academicorder { get; set; }

        public long acedemicyear { get; set; }

        public DateTime? academicyearstratdate { get; set; }
        public string type { get; set; }
        public Array WrittenTestSchedule { get; set; }

        public Array registrationList { get; set; }

        public Array admissioncatdrp { get; set; }

        public long CasteCategory_Id { get; set; }
        public DateTime prestartdate { get; set; }

        public DateTime presenddate { get; set; }

        public Array admissioncatdrpall { get; set; }

        public Array SubjectNames { get; set; }

        public Array SelectedSubjectNames { get; set; }

        public Array SubjectWiseWrittenMarks { get; set; }

        public Array MasterConfiguration { get; set; }

        public Array OralTestSchedule { get; set; }

        public int OralTestByPerson { get; set; }
        public Array WirettenTestSubjectWiseStudentMarks { get; set; }

        public int WrittenTestScheduleAppFlag { get; set; }

        public string SelctedDataMood { get; set; }

        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }

        public string searchString { get; set; }
        public string searchType { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int CurrentPageIndex { get; set; }
        public int TotalItems { get; set; }
        //public string searchString { get; set; }
        public string sortOrder { get; set; }
        //public string searchType { get; set; }


        //----------binding in single dto---------------//


        //----------binding in single dto---------------//

        //--subject details----//

        //public long PAMS_Id { get; set; }

        //public string PAMS_SubjectName { get; set; }

        //public decimal PAMS_MaxMarks { get; set; }

        //------------------//

        //------student details------//

        public long PASR_Id { get; set; }

        public string PASR_FirstName { get; set; }
        public string PASR_MiddleName { get; set; }
        public string PASR_LastName { get; set; }

        //-------------------------//

        //------marks details------//

        public decimal ObtMarks { get; set; }

        //-------------------------//

        public string From_Date { get; set; }
        public string To_Date { get; set; }
        public int ReportType { get; set; }

        public Array SearchstudentDetails { get; set; }

        // public TotalCountReportDTO[] SmsMailStudentDetails { get; set; }
        public string SmsMailText { get; set; }

        public string flagsubject { get; set; }
        public List<StudentApplicationDTO> SelectedStudentDetails { get; set; }
        public int count { get; set; }

        public string PASP_FirstName { get; set; }
        public string PASP_MiddleName { get; set; }
        public string PASP_LastName { get; set; }
        public string PASP_emailid { get; set; }
        public long PASP_MobileNo { get; set; }
        public long PASP_Phone { get; set; }
        public DateTime PASP_Date { get; set; }

        public string state { get; set; }

        public string PASP_ProspectusNo { get; set; }

        public EnquiryReportDTO[] SmsMailStudentDetails { get; set; }

        public TotalCountReportDTO[] SmsMailStudentDetailst { get; set; }

        public Array fillclass { get; set; }

        public bool Chq_config { get; set; }

        public long Id { get; set; }

        public long ISMS_Id { get; set; }

        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public decimal? ISMS_Max_Marks { get; set; }
        public decimal? ISMS_Min_Marks { get; set; }
        public long ISMS_ExamFlag { get; set; }
        public long ISMS_PreadmFlag { get; set; }
        public long ISMS_SubjectFlag { get; set; }
        public long ISMS_BatchAppl { get; set; }
        public long ISMS_ActiveFlag { get; set; }
        public long ISMS_OrderFlag { get; set; }
        public bool ISMS_TTFlag { get; set; }
        public bool ISMS_AttendanceFlag { get; set; }
        public int ISMS_LanguageFlg { get; set; }
        public int ISMS_AtExtraFeeFlg { get; set; }

        public long PAWTS_ID { get; set; }

        public Array allreports { get; set; }

        public long PASWMS_Id { get; set; }

        public decimal PASWMS_MarksScored { get; set; }
        public string PASWMS_PassFail { get; set; }

        public long PASWM_Id { get; set; }


        public Array _list_dto { get; set; }
        public Array _list_dto2 { get; set; }

        public List<WrittenTestMarksBindDataDTO> AllInOne { get; set; }

        public List<WrittenTestMarksBindDataDTO> AllInOne1 { get; set; }

        public Array totalcountDetails { get; set; }
        public Array Searchstudentheader { get; set; }

        public Array classlist { get; set; }

        public string classname { get; set; }
        public long ASMCL_Id { get; set; }
        public string order_type { get; set; }
        public decimal? totalMarks { get; set; }
        public decimal? totalObtain { get; set; }
        public int rank { get; set; }
        public string PASR_RegistrationNo { get; set; }
 
        public string siblingname { get; set; }
        public string siblingclass { get; set; }
        public string siblingadmno { get; set; }
        public string siblingsec { get; set; }

        public Array siblinglist { get; set; }

        public string success { get; set; }

        public string filterdata { get; set; }

        public WrittenTestMarksBindDataDTO[] data_array { get; set; }

        public long Mob_No { get; set; }

        public string Email_Id { get; set; }
    }
}
