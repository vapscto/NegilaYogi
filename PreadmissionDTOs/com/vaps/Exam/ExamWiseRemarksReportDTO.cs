using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class ExamWiseRemarksReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long EME_Id { get; set; }
        public long ECT_Id { get; set; }
        public long User_Id { get; set; }
        public long AMST_Id { get; set; }
        public long AMAY_RollNo { get; set; }
        public long ISMS_Id { get; set; }
        public string studentname { get; set; }
        public string AMST_AdmNo { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public string EMER_Remarks { get; set; }
        public string EMPATY_Color { get; set; }
        public string Report_Type { get; set; }
        public string Term_Type { get; set; }
        public string Subject_PT_TypeReport { get; set; }
        public string ECTERE_Remarks { get; set; }
        public Selected_EME_List[] Selected_EME_List { get; set; }
        public Selected_Term_List[] Selected_Term_List { get; set; }
        public Selected_Subject_List[] Selected_Subject_List { get; set; }
        public Array yearlist { get; set; }
        public Array classname { get; set; }
        public Array secname { get; set; }
        public Array examname { get; set; }
        public Array termlist { get; set; }
        public Array savedata { get; set; }
        public Array configuration { get; set; }
        public Array instituelist { get; set; }
        public Array getsubjectlist { get; set; }
    }

    public class Selected_EME_List
    {
        public long EME_Id { get; set; }
        public string EME_ExamName { get; set; }
    }

    public class Selected_Subject_List
    {
        public long ISMS_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
    }

    public class Selected_Term_List
    {
        public long ECT_Id { get; set; }
        public string ECT_TermName { get; set; }
    }
}
