using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class TT_Master_Subject_AbbreviationDTO
    {
        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }

        public long ISMS_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public decimal ISMS_Max_Marks { get; set; }
        public decimal ISMS_Min_Marks { get; set; }
        public bool ISMS_ExamFlag { get; set; }
        public bool ISMS_PreadmFlag { get; set; }
        public bool ISMS_SubjectFlag { get; set; }
        public bool ISMS_BatchAppl { get; set; }
        public bool ISMS_ActiveFlag { get; set; }
        public int ISMS_OrderFlag { get; set; }

        public long TTMSUAB_Id { get; set; }
        public string TTMSUAB_Abbreviation { get; set; }
        public bool TTMSUAB_ActiveFlag { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Array sujectslist { get; set; }
        public Array ttsujectslist { get; set; }
        public Array sujectslistedit { get; set; }
        public string subjectName { get; set; }
        public string academicyear { get; set; }

    }
}
