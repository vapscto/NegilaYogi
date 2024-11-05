using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class NAACInstShcrshipDTO
    {
        public long NCAC512INSCH_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC512INSCH_Year { get; set; }
        public string NCAC512INSCH_SchemeName { get; set; }
        public long NCAC512INSCH_NoOfStudents { get; set; }
        public string NCAC512INSCH_FileName { get; set; }
        public string NCAC512INSCH_FilePath { get; set; }
        public bool NCAC512INSCH_ActiveFlg { get; set; }
        public long NCAC512INSCH_CreatedBy { get; set; }
        public long NCAC512INSCH_UpdatedBy { get; set; }
        public DateTime NCAC512INSCH_CreatedDate { get; set; }
        public DateTime NCAC512INSCH_UpdatedDate { get; set; }


        public long UserId { get; set; }
        public long NCAC512INSCHF_Id { get; set; }

        public string ASMAY_Year { get; set; }
        public string NCAC512INSCH_StatusFlg { get; set; }

        public Array institutionlist { get; set; }
        public Array commentlist { get; set; }
        public Array allacademicyear { get; set; }
        public Array alldatalist { get; set; }
        public Array editfiles { get; set; }
        public Array editlist { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public NAACCriteriaFivefileDTO[] filelist { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }


    }
}
