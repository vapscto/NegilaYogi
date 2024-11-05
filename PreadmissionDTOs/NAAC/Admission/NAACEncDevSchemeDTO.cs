using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class NAACEncDevSchemeDTO
    {
        public long NCAC513INSCH_Id { get; set; }
        public long NCAC513INSCHF_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC513INSCH_ImpYear { get; set; }
        public string NCAC513INSCH_DevSchemeName { get; set; }
        public long NCAC513INSCH_NoOfStudents { get; set; }
        public string NCAC513INSCH_AgencyDetails { get; set; }
        public string NCAC513INSCH_StatusFlg { get; set; }
        public string NCAC513INSCH_FileName { get; set; }
        public string NCAC513INSCH_FilePath { get; set; }
        public bool NCAC513INSCH_ActiveFlg { get; set; }
        public long NCAC513INSCH_CreatedBy { get; set; }
        public long NCAC513INSCH_UpdatedBy { get; set; }
        public DateTime NCAC513INSCH_CreatedDate { get; set; }
        public DateTime NCAC513INSCH_UpdatedDate { get; set; }


        public long UserId { get; set; }

        public string ASMAY_Year { get; set; }

        public Array institutionlist { get; set; }
        public Array commentlist { get; set; }
        public Array allacademicyear { get; set; }
        public Array alldatalist { get; set; }
        public Array editlist { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array editfiles { get; set; }

        public NAACCriteriaFivefileDTO[] filelist { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
    }
}
