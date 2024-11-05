using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class ExamsubjectGroupMappingDTo
    {
        public int ESG_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int EMCA_Id { get; set; }
        public string ESG_SubjectGroupName { get; set; }
        public string ESG_ExamPromotionFlag { get; set; }
        public string ESG_CompulsoryFlag { get; set; }
        public decimal ESG_GroupMinMarks { get; set; }
        public decimal? ESG_GroupMaxMarks { get; set; }
        public bool ESG_ActiveFlag { get; set; }
        public int ESGE_Id { get; set; }
        public int EME_Id { get; set; }
        public bool ESGE_ActiveFlag { get; set; }
        public int ESGS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public bool ESGS_ActiveFlag { get; set; }
        public Array getyear { get; set; }
        public Array getcategory { get; set; }
        public Array getexam { get; set; }
        public Array getsubject { get; set; }
        public string EMCA_CategoryName { get; set; }
        public string EME_ExamName { get; set; }
        public long EME_ExamOrder { get; set; }
        public string ISMS_SubjectName { get; set; }
        public long ISMS_OrderFlag { get; set; }
        public get_subjectlist[] get_subjectlist { get; set; }
        public string grpname { get; set; }
        public string percentage { get; set; }
        public string Flag { get; set; }
        public string Compulsory1 { get; set; }
        public string Promotion1 { get; set; }
        public string message { get; set; }
        public Array getdetails { get; set; }
        public Array editdata { get; set; }
        public string Flag1 { get; set; }
        public string Flag2 { get; set; }
        public Array viewdata { get; set; }
        public bool returnval { get; set; }
        public bool appornonresult { get; set; }
        public string ASMAY_Year { get; set; }
        public int ASMAY_Order { get; set; }

    }
    public class get_subjectlist
    {
        public long ISMS_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
    }
}
