using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
   public class NAAC_AC_711_GenderEquity_DTO : CommonParamDTO
    {
        public long NCAC711GENEQ_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC711GENEQ_Year { get; set; }
        public string NCAC711GENEQ_ProgramTitle { get; set; }
        public DateTime? NCAC711GENEQ_FromDate { get; set; }
        public DateTime? NCAC711GENEQ_ToDate { get; set; }
        public long NCAC711GENEQ_NoOfParticipantsMale { get; set; }
        public long NCAC711GENEQ_NoOfParticipantsFeMale { get; set; }
        public string NCAC711GENEQF_Filedesc { get; set; }
        public string NCAC711GENEQF_FileName { get; set; }
        public string NCAC711GENEQF_FilePath { get; set; }
        public bool? NCAC711GENEQ_ActiveFlg { get; set; }
        public long NCAC711GENEQ_CreatedBy { get; set; }
        public long NCAC711GENEQ_UpdatedBy { get; set; }
        public long NCAC711GENEQ_CreatedDate { get; set; }
        public long NCAC711GENEQ_UpdatedDate { get; set; }
        public long NCAC711GENEQF_Id { get; set; }


        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public long UserId { get; set; }
        public string retrunMsg { get; set; }

        public Array allacademicyear { get; set; }
        public Array alldatalist { get; set; }
        public Array editlisttab1 { get; set; }
        public Array editfilelist { get; set; }
        public NAAC_AC_711_GenderEquity_DTO[] NAACAC711GenderEquityDTO { get; set; }
        public List<long> selectedYear { get; set; }
        public string ASMAY_Year { get; set; }
        public Array alldata { get; set; }
        public Array alldatafile { get; set; }
        public string MI_Name { get; set; }
        public Array institutionlist { get; set; }
        public Array commentlist { get; set; }
        public Array commentlist1 { get; set; }
        public Array view { get; set; }
        public string NCAC711GENEQC_Remarks { get; set; }
        public long NCAC711GENEQC_Id { get; set; }
        public long NCAC711GENEQC_RemarksBy { get; set; }
        public string NCAC711GENEQC_StatusFlg { get; set; }
        public bool NCAC711GENEQC_ActiveFlag { get; set; }
        public long NCAC711GENEQC_CreatedBy { get; set; }
        public DateTime? NCAC711GENEQC_CreatedDate { get; set; }
        public long NCAC711GENEQC_UpdatedBy { get; set; }
        public DateTime? NCAC711GENEQC_UpdatedDate { get; set; }
        public string UserName { get; set; }
        public string NCAC711GENEQFC_Remarks { get; set; }
        public long NCAC711GENEQFC_Id { get; set; }
        public long NCAC711GENEQFC_RemarksBy { get; set; }
        public string NCAC711GENEQFC_StatusFlg { get; set; }
        public string Remarks { get; set; }
        public bool NCAC711GENEQFC_ActiveFlag { get; set; }
        public long NCAC711GENEQFC_CreatedBy { get; set; }
        public DateTime? NCAC711GENEQFC_CreatedDate { get; set; }
        public long NCAC711GENEQFC_UpdatedBy { get; set; }
        public long filefkid { get; set; }
        public DateTime? NCAC711GENEQFC_UpdatedDate { get; set; }
        public string NCAC711GENEQ_StatusFlg { get; set; }
    }
}
