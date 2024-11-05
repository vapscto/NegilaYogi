using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class exammasterDTO
    {
        public bool already_cnt { get; set; }
        public int EME_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public string EME_ExamName { get; set; }
        public string EME_ExamCode { get; set; }
        public string EME_IVRSExamName { get; set; }
        public string EME_ExamDescription { get; set; }
        public int EME_ExamOrder { get; set; }
        public bool EME_FinalExamFlag { get; set; }
        public bool EME_ActiveFlag { get; set; }
        public Array exammastername { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public Array editlist { get; set; }
        public string retrunMsg { get; set; }
        public exammasterDTO[] examDTO { get; set; }


        //Master Exam Paper Type
        public int EMPATY_Id { get; set; }
        public string EMPATY_PaperTypeName { get; set; }
        public string EMPATY_PaperTypeDescription { get; set; }
        public string EMPATY_Color { get; set; }
        public bool? EMPATY_ActiveFlag { get; set; }
        public Array GetExamPTLoadDetails { get; set; }
        public Array GetExamPTEditDetails { get; set; }
    }
}
