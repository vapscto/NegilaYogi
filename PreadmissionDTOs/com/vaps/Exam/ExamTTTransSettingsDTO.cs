using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class ExamTTTransSettingsDTO
    {
        public int EMG_Id { get; set; }
        public string EMG_GroupName { get; set; }
        public string ISMS_SubjectName { get; set; }
        public int EMSS_Id { get; set; }
        public string EMSS_SubSubjectName { get; set; }
        public Array Acdlist { get; set; }
        public Array catlist { get; set; }
        public Array ctlist { get; set; }
        public Array seclist { get; set; }
        public Array examlist { get; set; }
        public Array subject_group { get; set; }
        public Array subject_name { get; set; }
        public Array time_slot { get; set; }
        public Array SubSubject { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public string check_save { get; set; }
        public ExamTTTransSettingsDTO[] TempararyArrayList { get; set; }
        public Array detailslist { get; set; }
        public string academicyear { get; set; }
        public string classname { get; set; }
        public string sectionname { get; set; }
        public string examname { get; set; }
        public Array viewdata { get; set; }
        public string subjectName { get; set; }
        public DateTime? slotdate { get; set; }
        public string slotname { get; set; }
        public Array listedit { get; set; }
        public string ETTS_SessionName { get; set; }        
        public int EXTTS_Id { get; set; }
        public int EXTT_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long ETTS_Id { get; set; }
        public DateTime? EXTTS_Date { get; set; }
        public string EXTTS_ExamDuration { get; set; }
        public string EXTTS_FromTime { get; set; }
        public string EXTTS_EndTime { get; set; }
        public bool EXTTS_ActiveFlag { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public int EME_Id { get; set; }
        public bool EXTT_ActiveFlag { get; set; }
        public DateTime? EXTT_FromDate { get; set; }
        public DateTime? EXTT_EndDate { get; set; }
        public string ASMCL_ClassName { get; set; }
        public int ASMCL_Order { get; set; }
        public int ASMC_Order { get; set; }
        public int ASMAY_Order { get; set; }


    }
}
