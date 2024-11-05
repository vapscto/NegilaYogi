using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission.Criteria7
{
   public class LocalCommunityDTO
    {
        public LocalCommunityDTO[] NAACAC711LocalCommunityDTO { get; set;}
        public long NCAC7111LOCCOM_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC7111LOCCOM_Year { get; set; }
        public string NCAC7111LOCCOM_NoOfAddress { get; set; }
        public long NCAC7111LOCCOM_NoOfEngage { get; set; }
        public DateTime? NCAC7111LOCCOM_Date { get; set; }
        public long NCAC7111LOCCOM_Duration { get; set; }
        public string NCAC7111LOCCOM_InitiativeName { get; set; }
        public string NCAC7111LOCCOM_IssuesAddressed { get; set; }
        public long NCAC7111LOCCOM_NoOfParticipant { get; set; }
        public bool NCAC7111LOCCOM_ActiveFlg { get; set; }
        public long NCAC7111LOCCOM_CreatedBy { get; set; }
        public long NCAC7111LOCCOM_UpdatedBy { get; set; }
        public DateTime NCAC7111LOCCOM_CreatedDate { get; set; }
        public DateTime NCAC7111LOCCOM_UpdatedDate { get; set; }

        public string NCAC7111LOCCOMF_Filedesc { get; set; }
        public string NCAC7111LOCCOMF_FileName { get; set; }
        public string NCAC7111LOCCOMF_FilePath { get; set; }
        public long NCAC7111LOCCOMF_Id { get; set; }



        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public long UserId { get; set; }

        public Array allacademicyear { get; set; }
        public Array alldata { get; set; }
        public Array editlisttab1 { get; set; }
        public Array alldatalist { get; set; }
        public Array editfilelist { get; set; }
        public string ASMAY_Year { get; set; }
        public string MI_Name { get; set; }
        public Array institutionlist { get; set; }
        public Array commentlist { get; set; }
        public Array commentlist1 { get; set; }
        public Array view { get; set; }
        public string NCAC7111LOCCOMC_Remarks { get; set; }
        public long NCAC7111LOCCOMC_Id { get; set; }
        public long NCAC7111LOCCOMC_RemarksBy { get; set; }
        public string NCAC7111LOCCOMC_StatusFlg { get; set; }
        public bool NCAC7111LOCCOMC_ActiveFlag { get; set; }
        public long NCAC7111LOCCOMC_CreatedBy { get; set; }
        public DateTime? NCAC7111LOCCOMC_CreatedDate { get; set; }
        public long NCAC7111LOCCOMC_UpdatedBy { get; set; }
        public DateTime? NCAC7111LOCCOMC_UpdatedDate { get; set; }
        public string UserName { get; set; }
        public string NCAC7111LOCCOMFC_Remarks { get; set; }
        public long NCAC7111LOCCOMFC_Id { get; set; }
        public long NCAC7111LOCCOMFC_RemarksBy { get; set; }
        public string NCAC7111LOCCOMFC_StatusFlg { get; set; }
        public bool NCAC7111LOCCOMFC_ActiveFlag { get; set; }
        public long NCAC7111LOCCOMFC_CreatedBy { get; set; }
        public DateTime? NCAC7111LOCCOMFC_CreatedDate { get; set; }
        public long NCAC7111LOCCOMFC_UpdatedBy { get; set; }
        public long filefkid { get; set; }
        public string Remarks { get; set; }
        public DateTime? NCAC7111LOCCOMFC_UpdatedDate { get; set; }
        public string NCAC7111LOCCOM_StatusFlg { get; set; }
    }
}
