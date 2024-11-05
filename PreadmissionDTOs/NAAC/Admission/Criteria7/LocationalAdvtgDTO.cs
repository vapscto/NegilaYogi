using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission.Criteria7
{
   public class LocationalAdvtgDTO : CommonParamDTO
    {
        public LocationalAdvtgDTO[] NAACAC711LocationalAdvtgDTO { get; set; }
        public long NCAC7110LOCADVTG_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC7110LOCADVTG_Year { get; set; }
        public long NCAC7110LOCADVTG_NoOfAddress { get; set; }
        public long NCAC7110LOCADVTG_NoOfEngage { get; set; }
        public DateTime? NCAC7110LOCADVTG_Date { get; set; }
        public long NCAC7110LOCADVTG_Duration { get; set; }
        public string NCAC7110LOCADVTG_InitiativeName { get; set; }
        public string NCAC7110LOCADVTG_IssuesAddressed { get; set; }
        public long NCAC7110LOCADVTG_NoOfParticipant { get; set; }
        public bool NCAC7110LOCADVTG_ActiveFlg { get; set; }
        public long NCAC7110LOCADVTG_CreatedBy { get; set; }
        public long NCAC7110LOCADVTG_UpdatedBy { get; set; }
        public long NCAC7110LOCADVTGF_Id { get; set; }
        public DateTime NCAC7110LOCADVTG_CreatedDate { get; set; }
        public DateTime NCAC7110LOCADVTG_UpdatedDate { get; set; }
        public string NCAC7110LOCADVTGF_FileName { get; set; }
        public string NCAC7110LOCADVTGF_Filedesc { get; set; }
        public string NCAC7110LOCADVTGF_FilePath { get; set; }
        public string NCAC7110LOCADVTG_StatusFlg { get; set; }
        public string NCAC7110LOCADVTGF_StatusFlg { get; set; }
        public string Remarks { get; set; }

        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public long UserId { get; set; }
        public long filefkid { get; set; }

        public Array commentlist { get; set; }
        public Array commentlist1 { get; set; }
        public Array view { get; set; }
        public Array allacademicyear { get; set; }
        public Array alldata { get; set; }
        public Array editlisttab1 { get; set; }
        public Array alldatalist { get; set; }
        public Array editfilelist { get; set; }
        public string ASMAY_Year { get; set; }
        public string MI_Name { get; set; }
        public Array institutionlist { get; set; }
        public string NCAC7110LOCADVTGC_Remarks { get; set; }
        public long NCAC7110LOCADVTGC_Id { get; set; }
        public long NCAC7110LOCADVTGC_RemarksBy { get; set; }
        public string NCAC7110LOCADVTGC_StatusFlg { get; set; }
        public bool NCAC7110LOCADVTGC_ActiveFlag { get; set; }
        public long NCAC7110LOCADVTGC_CreatedBy { get; set; }
        public DateTime? NCAC7110LOCADVTGC_CreatedDate { get; set; }
        public long NCAC7110LOCADVTGC_UpdatedBy { get; set; }
        public DateTime? NCAC7110LOCADVTGC_UpdatedDate { get; set; }
        public string UserName { get; set; }
        public string NCAC7110LOCADVTGFC_Remarks { get; set; }
        public long NCAC7110LOCADVTGFC_Id { get; set; }
        public long NCAC7110LOCADVTGFC_RemarksBy { get; set; }
        public string NCAC7110LOCADVTGFC_StatusFlg { get; set; }
        public bool NCAC7110LOCADVTGFC_ActiveFlag { get; set; }
        public long NCAC7110LOCADVTGFC_CreatedBy { get; set; }
        public DateTime? NCAC7110LOCADVTGFC_CreatedDate { get; set; }
        public long NCAC7110LOCADVTGFC_UpdatedBy { get; set; }
        public DateTime? NCAC7110LOCADVTGFC_UpdatedDate { get; set; }
        
    }
}
