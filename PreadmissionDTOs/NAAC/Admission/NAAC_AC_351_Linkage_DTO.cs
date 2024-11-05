using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
   public class NAAC_AC_351_Linkage_DTO
    {

        public long NCAC351LIN_Id { get; set; }
        public long MI_Id { get; set; }
        public string NCAC351LIN_LinkageTitle { get; set; }
        public string NCAC351LIN_PartnerName { get; set; }
        public long NCAC351LIN_CommYear { get; set; }
        public DateTime NCAC351LIN_From { get; set; }
        public DateTime NCAC351LIN_To { get; set; }
        public string NCAC351LIN_LinkageNature { get; set; }
        public string NCAC351LIN_LinkOfDocument { get; set; }
        public string NCAC351LIN_FileName { get; set; }
        public string NCAC351LIN_FilePath { get; set; }
        public bool NCAC351LIN_ActiveFlg { get; set; }
        public long NCAC351LIN_CreatedBy { get; set; }
        public long NCAC351LIN_UpdatedBy { get; set; }
        public DateTime NCAC351LIN_CreatedDate { get; set; }
        public DateTime NCAC351LIN_UpdatedDate { get; set; }
        public Array yearlist { get; set; }
        public Array allacademicyear { get; set; }
        public bool duplicate { get; set; }
        public long UserId { get; set; }
        public string msg { get; set; }
        public Array alldata1 { get; set; }
        public string ASMAY_Year { get; set; }
        public long ASMAY_Id { get; set; }
        public Array editlist { get; set; }
        public bool returnval { get; set; }
        public NAAC_AC_351_Linkage_DTO[] filelist { get; set; }
        public NAAC_AC_351_Linkage_DTO[] remv2 { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public Array editFileslist { get; set; }
        public Array viewuploadflies { get; set; }
        public long NCAC351LINF_Id { get; set; }
        public Array institutionlist { get; set; }
        public Array commentlist { get; set; }
        public long NCAC351LINC_Id { get; set; }
        public string NCAC351LINC_Remarks { get; set; }
        public long? NCAC351LINC_RemarksBy { get; set; }
        public string NCAC351LINC_StatusFlg { get; set; }
        public bool? NCAC351LINC_ActiveFlag { get; set; }
        public long? NCAC351LINC_CreatedBy { get; set; }
        public DateTime? NCAC351LINC_CreatedDate { get; set; }
        public long? NCAC351LINC_UpdatedBy { get; set; }
        public DateTime? NCAC351LINC_UpdatedDate { get; set; }
        public string UserName { get; set; }
        public Array commentlist1 { get; set; }
        public long NCAC351LINFC_Id { get; set; }
        public string NCAC351LINFC_Remarks { get; set; }
        public long? NCAC351LINFC_RemarksBy { get; set; }
        public bool? NCAC351LINFC_ActiveFlag { get; set; }
        public long? NCAC351LINFC_CreatedBy { get; set; }
        public DateTime? NCAC351LINFC_CreatedDate { get; set; }
        public long? NCAC351LINFC_UpdatedBy { get; set; }
        public DateTime? NCAC351LINFC_UpdatedDate { get; set; }
        public string NCAC351LINFC_StatusFlg { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
        public string NCAC351LIN_StatusFlg { get; set; }
        public string NCAC351LINF_StatusFlg { get; set; }
        public bool NCAC351LINF_ActiveFlg { get; set; }
        public Array remvfile { get; set; }
        public Array orig { get; set; }
    }
}
