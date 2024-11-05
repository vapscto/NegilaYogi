using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class NAACCompExamsDTO
    {
        public long NCAC514CPEX_Id { get; set; }
        public long NCAC514CPEXF_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC514CPEX_ImpYear { get; set; }
        public string NCAC514CPEX_ExamSchemeName { get; set; }
        public long NCAC514CPEX_NoOfStudents { get; set; }
        public string NCAC514CPEX_FileName { get; set; }
        public string NCAC514CPEX_StatusFlg { get; set; }
        public string NCAC514CPEX_FilePath { get; set; }
        public bool NCAC514CPEX_ActiveFlg { get; set; }
        public long NCAC514CPEX_CreatedBy { get; set; }
        public long NCAC514CPEX_UpdatedBy { get; set; }
        public DateTime NCAC514CPEX_CreatedDate { get; set; }
        public DateTime NCAC514CPEX_UpdatedDate { get; set; }
        public long UserId { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
        public Array commentlist { get; set; }
        public string ASMAY_Year { get; set; }

        public Array allacademicyear { get; set; }
        public Array institutionlist { get; set; }
        public Array alldatalist { get; set; }
        public Array editlist { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }

        public Array editfiles { get; set; }

        public NAACCriteriaFivefileDTO[] filelist { get; set; }

    }
}
