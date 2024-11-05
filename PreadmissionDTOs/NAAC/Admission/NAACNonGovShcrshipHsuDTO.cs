using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class NAACNonGovShcrshipHsuDTO
    {
        public long NCAC512NGSCH_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC512NGSCH_Year { get; set; }
        public string NCAC512NGSCH_SchemeName { get; set; }
        public long NCAC512NGSCH_NoOfStudents { get; set; }
        public bool NCAC512NGSCH_ActiveFlg { get; set; }
        public long NCAC512NGSCH_CreatedBy { get; set; }
        public long NCAC512NGSCH_UpdatedBy { get; set; }
        public DateTime NCAC512NGSCH_CreatedDate { get; set; }
        public DateTime NCAC512NGSCH_UpdatedDate { get; set; }
        public string NCAC512NGSCH_StatusFlg { get; set; }

        public long UserId { get; set; }
        public long NCAC512NGSCHF_Id { get; set; }

        public string ASMAY_Year { get; set; }

        public Array institutionlist { get; set; }
        public Array allacademicyear { get; set; }
        public Array alldatalist { get; set; }
        public Array editfiles { get; set; }
        public Array editlist { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public NAACCriteriaFivefileDTO[] filelist { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
        public Array commentlist { get; set; }

    }
}
