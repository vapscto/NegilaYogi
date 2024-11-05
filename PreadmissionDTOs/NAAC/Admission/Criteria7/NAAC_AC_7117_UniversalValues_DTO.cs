using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
   public class NAAC_AC_7117_UniversalValues_DTO : CommonParamDTO
    {
        public long NCAC7117UNIVAL_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC7117UNIVAL_Year { get; set; }
        public string NCAC7117UNIVAL_ProgramTitle { get; set; }
        public long NCAC7117UNIVAL_NoOfPartcipants { get; set; }
        public DateTime NCAC7117UNIVAL_FromDate { get; set; }
        public DateTime NCAC7117UNIVAL_ToDate { get; set; }
        public string NCAC7117UNIVALF_Filedesc { get; set; }
        public string NCAC7117UNIVALF_FileName { get; set; }
        public string NCAC7117UNIVALF_FilePath { get; set; }
        public bool NCAC7117UNIVAL_ActiveFlg { get; set; }
        public long NCAC7117UNIVAL_CreatedBy { get; set; }
        public long NCAC7117UNIVAL_UpdatedBy { get; set; }
        public DateTime? NCAC7117UNIVAL_CreatedDate { get; set; }
        public DateTime? NCAC7117UNIVAL_UpdatedDate { get; set; }
        public long NCAC7117UNIVALF_Id { get; set; }

        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public long UserId { get; set; }
        public string retrunMsg { get; set; }

        public Array allacademicyear { get; set; }
        public Array alldatalist { get; set; }
        public Array editlisttab1 { get; set; }
        public Array editfilelist { get; set; }
        public NAAC_AC_7117_UniversalValues_DTO[] NAACAC7117DTO { get; set; }
        public string ASMAY_Year { get; set; }
        public string MI_Name { get; set; }
        public Array institutionlist { get; set; }
        public long NCAC7117UNIVALC_Id { get; set; }
        public string NCAC7117UNIVALC_Remarks { get; set; }
        public long? NCAC7117UNIVALC_RemarksBy { get; set; }
        public string NCAC7117UNIVALC_StatusFlg { get; set; }
        public bool? NCAC7117UNIVALC_ActiveFlag { get; set; }
        public long? NCAC7117UNIVALC_CreatedBy { get; set; }
        public DateTime? NCAC7117UNIVALC_CreatedDate { get; set; }
        public long? NCAC7117UNIVALC_UpdatedBy { get; set; }
        public DateTime? NCAC7117UNIVALC_UpdatedDate { get; set; }
        public Array commentlist { get; set; }
        public string UserName { get; set; }
        public long NCAC7117UNIVALFC_Id { get; set; }
        public string NCAC7117UNIVALFC_Remarks { get; set; }
        public long? NCAC7117UNIVALFC_RemarksBy { get; set; }
        public bool? NCAC7117UNIVALFC_ActiveFlag { get; set; }
        public long? NCAC7117UNIVALFC_CreatedBy { get; set; }
        public DateTime? NCAC7117UNIVALFC_CreatedDate { get; set; }
        public long? NCAC7117UNIVALFC_UpdatedBy { get; set; }
        public DateTime? NCAC7117UNIVALFC_UpdatedDate { get; set; }
        public string NCAC7117UNIVALFC_StatusFlg { get; set; }
        public Array commentlist1 { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
        public string NCAC7117UNIVAL_StatusFlg { get; set; }
        public string NCAC7117UNIVALF_StatusFlg { get; set; }
        public bool NCAC7117UNIVALF_ActiveFlg { get; set; }
    }
}
