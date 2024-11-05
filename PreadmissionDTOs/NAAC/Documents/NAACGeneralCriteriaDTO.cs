using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Documents
{
    public class NAACGeneralCriteriaDTO
    {
        public long NCACCRGEN_Id { get; set; }
        public long NCACCRGENLI_Id { get; set; }
        public long NCACCRGENF_Id { get; set; }
        public long MI_Id { get; set; }
        public long MT_Id { get; set; }
        public long NAACSL_Id { get; set; }
        public string NCACCRGEN_CriteriaDescription { get; set; }
        public bool NCACCRGEN_ActiveFlg { get; set; }
        public long NCACCRGEN_CreatedBy { get; set; }
        public long NCACCRGEN_UpdatedBy { get; set; }
        public DateTime NCACCRGEN_CreatedDate { get; set; }
        public DateTime NCACCRGEN_UpdatedDate { get; set; }
        public long UserId { get; set; }

        public string ASMAY_Year { get; set; }

        public Array allacademicyear { get; set; }
        public Array criterialist { get; set; }
        public Array editfiles { get; set; }
        public Array alldatalist { get; set; }
        public Array editlink { get; set; }
        public Array editlist { get; set; }
        public Array institutionlist { get; set; }
        public Array mtlist { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
   
        public string NAACSL_SLNo { get; set; }
        public string NAACSL_SLNoDescription { get; set; }
        public NAACCriteriaFivefileDTO[] filelist { get; set; }
        public linkdto[] linklist { get; set; }

    }

    public class linkdto {
        public long gridid { get; set; }
        public long linkid { get; set; }
        public string linkname { get; set; }
        public string linkdesc { get; set; }
    }
   
}
