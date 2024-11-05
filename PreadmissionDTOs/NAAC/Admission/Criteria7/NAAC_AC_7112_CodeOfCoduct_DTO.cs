using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
   public class NAAC_AC_7112_CodeOfCoduct_DTO 
    {
        public long NCAC7112CODCON_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC7112CODCON_Year { get; set; }
        public string NCAC7112CODCON_URL { get; set; }
        public string NCAC7112CODCONF_Filedesc { get; set; }
        public string NCAC7112CODCONF_FileName { get; set; }
        public string NCAC7112CODCONF_FilePath { get; set; }
        public bool NCAC7112CODCON_ActiveFlg { get; set; }
        public long NCAC7112CODCON_CreatedBy { get; set; }
        public long NCAC7112CODCON_UpdatedBy { get; set; }
        public DateTime NCAC7112CODCON_CreatedDate { get; set; }
        public DateTime NCAC7112CODCON_UpdatedDate { get; set; }
        public Array allacademicyear { get; set; }
        public Array alldata { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public long UserId { get; set; }
        public string NCAC7112CODCON_StatusFlg { get; set; }
        public Array alldatalist { get; set; }
        public Array edit1 { get; set; }
        public Array editfilelist { get; set; }
        public Array editlisttab1 { get; set; }
        public Array view { get; set; }
        public Array fileapprovedlist { get; set; }
        public long NCAC7112CODCONF_Id { get; set; }
        public NAAC_AC_7112_CodeOfCoduct_DTO[] filelist { get; set; }
        public string MI_Name { get; set; }
        public string statusflg { get; set; }
        public Array institutionlist { get; set; }
        public Array commentlist { get; set; }
        public string UserName { get; set; }
        public long NCAC7112CODCONC_Id { get; set; }
        public string NCAC7112CODCONC_Remarks { get; set; }
        public long? NCAC7112CODCONC_RemarksBy { get; set; }
        public string NCAC7112CODCONC_StatusFlg { get; set; }
        public bool? NCAC7112CODCONC_ActiveFlag { get; set; }
        public long? NCAC7112CODCONC_CreatedBy { get; set; }
        public DateTime? NCAC7112CODCONC_CreatedDate { get; set; }
        public long? NCAC7112CODCONC_UpdatedBy { get; set; }
        public DateTime? NCAC7112CODCONC_UpdatedDate { get; set; }
        public string Remarks { get; set; }
        public Array commentlist1 { get; set; }
        public long NCAC7112CODCONFC_Id { get; set; }
        public string NCAC7112CODCONFC_Remarks { get; set; }
        public long? NCAC7112CODCONFC_RemarksBy { get; set; }
        public bool? NCAC7112CODCONFC_ActiveFlag { get; set; }
        public long? NCAC7112CODCONFC_CreatedBy { get; set; }
        public DateTime? NCAC7112CODCONFC_CreatedDate { get; set; }
        public long? NCAC7112CODCONFC_UpdatedBy { get; set; }
        public DateTime? NCAC7112CODCONFC_UpdatedDate { get; set; }
        public string NCAC7112CODCONFC_StatusFlg { get; set; }
        public long filefkid { get; set; }
        public string NCAC7112CODCONF_StatusFlg { get; set; }

    }
}
