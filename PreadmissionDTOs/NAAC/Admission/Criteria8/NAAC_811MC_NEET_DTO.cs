using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission.Criteria8
{
   public class NAAC_811MC_NEET_DTO
    {
        public long NCMC811NEET_Id { get; set; }
        public long NCMC811NEETF_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public Array institutionlist { get; set; }
        public string ASMAY_Year { get; set; }
        public long NCMC811NEET_Year { get; set; }
        public long NCMC811NEET_NoOfStudentsEnrolled { get; set; }
        public string NCMC811NEET_Range { get; set; }
        public decimal NCMC811NEET_Mean { get; set; }
        public decimal NCMC811NEET_StandardDeviation { get; set; }
        public bool NCMC811NEET_ActiveFlg { get; set; }
        public long NCMC811NEET_CreatedBy { get; set; }
        public long NCMC811NEET_UpdatedBy { get; set; }
        public DateTime NCMC811NEET_CreatedDate { get; set; }
        public DateTime NCMC811NEET_UpdatedDate { get; set; }
        public Array yearlist { get; set; }
        public Array alldata { get; set; }
        public long UserId { get; set; }
        public string msg { get; set; }
        public bool returnval { get; set; }
        public bool NCMC811NEETF_ActiveFlg { get; set; }
        public Array editlist { get; set; }
        public Array editFileslist { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public Array viewuploadflies { get; set; }
        public long count { get; set; }
        public long count1 { get; set; }
        

        public Naac_CommonFiles_DTO[] filelist { get; set; }
        public Array getinstitutioncycle { get; set; }
        public Array getinstitution { get; set; }
        public string NAACSL_InstitutionTypeFlg { get; set; }
        public NAAC_811MC_NEET_DTO[] selected_Inst { get; set; }
        public long cycleid { get; set; }
        public Array reportlist { get; set; }
        public Array reportlist2 { get; set; }
        public Array commentlist { get; set; }
        public Array commentlist1 { get; set; }
        public string NCMC811NEETC_Remarks { get; set; }
        public long NCMC811NEETC_Id { get; set; }
        public string NCMC811NEETC_StatusFlg { get; set; }
        public long NCMC811NEETC_RemarksBy { get; set; }
        public bool NCMC811NEETC_ActiveFlag { get; set; }
        public long NCMC811NEETC_CreatedBy { get; set; }
        public DateTime? NCMC811NEETC_CreatedDate { get; set; }
        public long NCMC811NEETC_UpdatedBy { get; set; }
        public DateTime? NCMC811NEETC_UpdatedDate { get; set; }
        public string UserName { get; set; }
        public string NCMC811NEETFC_Remarks { get; set; }
        public long NCMC811NEETFC_Id { get; set; }
        public long NCMC811NEETFC_RemarksBy { get; set; }
        public string NCMC811NEETFC_StatusFlg { get; set; }
        public bool NCMC811NEETFC_ActiveFlag { get; set; }
        public long NCMC811NEETFC_CreatedBy { get; set; }
        public DateTime? NCMC811NEETFC_CreatedDate { get; set; }
        public long NCMC811NEETFC_UpdatedBy { get; set; }
        public long filefkid { get; set; }
        public DateTime? NCMC811NEETFC_UpdatedDate { get; set; }
        public string Remarks { get; set; }
    }
}
