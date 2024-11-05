using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Exam
{
    public class HallTicketGenerationCollegeDTO
    {
        public long ASMAY_Id { get; set; }
        public long MI_Id { get; set; }
        public bool returnval { get; set; }
        public Array Acdlist { get; set; }
        public Array examlist { get; set; }

        public long EHTC_Id { get; set; }
      
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long AMCST_Id { get; set; }
        public string EHTC_HallTicketNo { get; set; }
        public bool EHTC_PublishFlg { get; set; }
        public bool EHTC_ActiveFlag { get; set; }
        public Array courseslist { get; set; }
        public Array branchlist { get; set; }
        public Array semisters { get; set; }
        public Array sectionlist { get; set; }
        public Array Alldata { get; set; }
        public Array AllDataTimeTable { get; set; }
        //section_Array
        public selectedstudentsCollege[] section_Array { get; set; }
        public  long EME_Id { get; set; }
        public string prefixstr { get; set; }
        public string startno { get; set; }
        public string increment { get; set; }
        public string leadingzeros { get; set; }
        public Array GetStudent { get; set; }
        public string htmldata { get; set; }
        public Array subjectshemalist { get; set; }
        public Array view_exam_subjects { get; set; }
        public long ACSS_Id { get; set; }
        public long ACST_Id { get; set; }
        public Array time_slot { get; set; }
        public long EXTTC_Id { get; set; }
        public string returnduplicatestatus { get; set; }
        public DateTime? EXTTC_FromDateTime { get; set; }
        public DateTime? EXTTC_ToDateTime { get; set; }
        public CollegeTTTransSettingsDTO[] TempararyArrayList { get; set; }
        public string FlagReport { get; set; }
        public Array OnEditSubjects { get; set; }
    }
    public class selectedstudentsCollege
    {
        public long AMCST_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long EHTC_Id { get; set; }
    }
    public class CollegeTTTransSettingsDTO
    {
        public long ISMS_Id { get; set; }
        public string check_save { get; set; }
        public long EMTTSC_Id { get; set; }
        public DateTime? EXTTSC_Date { get; set; }
        public string EXTTC_ExaminationCenter { get; set; }
        public string EXTTC_ReportingTime { get; set; }
    }
}
