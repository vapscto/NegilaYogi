using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
   public class NAAC_AC_7115_ProfessionalEthics_DTO : CommonParamDTO
    {
        public long NCAC7115PROETH_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC7115PROETH_Year { get; set; }
        public string NCAC7115PROETH_URL { get; set; }
        public string NCAC7115PROETHF_Filedesc { get; set; }
        public string NCAC7115PROETHF_FileName { get; set; }
        public string NCAC7115PROETHF_FilePath { get; set; }
        public bool NCAC7115PROETH_ActiveFlg { get; set; }
        public long NCAC7115PROETH_CreatedBy { get; set; }
        public long NCAC7115PROETH_UpdatedBy { get; set; }
        public DateTime NCAC7115PROETH_CreatedDate { get; set; }
        public DateTime NCAC7115PROETH_UpdatedDate { get; set; }
        public long NCAC7115PROETHF_Id { get; set; }

        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public long UserId { get; set; }
        public string retrunMsg { get; set; }

        public Array allacademicyear { get; set; }
        public Array alldatalist { get; set; }
        public Array editlisttab1 { get; set; }
        public Array editfilelist { get; set; }
        public NAAC_AC_7115_ProfessionalEthics_DTO[] NAACAC7115DTO { get; set; }
        public string ASMAY_Year { get; set; }
        public string MI_Name { get; set; }
        public Array institutionlist { get; set; }


        public Array commentlist { get; set; }
        public long NCAC7115PROETHC_Id { get; set; }
        public string NCAC7115PROETHC_Remarks { get; set; }
        public long? NCAC7115PROETHC_RemarksBy { get; set; }
        public string NCAC7115PROETHC_StatusFlg { get; set; }
        public bool? NCAC7115PROETHC_ActiveFlag { get; set; }
        public long? NCAC7115PROETHC_CreatedBy { get; set; }
        public DateTime? NCAC7115PROETHC_CreatedDate { get; set; }
        public long? NCAC7115PROETHC_UpdatedBy { get; set; }
        public DateTime? NCAC7115PROETHC_UpdatedDate { get; set; }
        public long NCAC7115PROETHFC_Id { get; set; }
        public string NCAC7115PROETHFC_Remarks { get; set; }
        public long? NCAC7115PROETHFC_RemarksBy { get; set; }
        public bool? NCAC7115PROETHFC_ActiveFlag { get; set; }
        public long? NCAC7115PROETHFC_CreatedBy { get; set; }
        public DateTime? NCAC7115PROETHFC_CreatedDate { get; set; }
        public long? NCAC7115PROETHFC_UpdatedBy { get; set; }
        public DateTime? NCAC7115PROETHFC_UpdatedDate { get; set; }
        public string NCAC7115PROETHFC_StatusFlg { get; set; }
        public string NCAC7115PROETH_StatusFlg { get; set; }
        public string NCAC7115PROETHF_StatusFlg { get; set; }
        public bool NCAC7115PROETHF_ActiveFlg { get; set; }
        public string UserName { get; set; }
        public Array commentlist1 { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
    }
}
