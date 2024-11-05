using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
   public class NAAC_AC_718_WasteManagement_DTO : CommonParamDTO
    {
        public long NCAC718WAMAN_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC718WAMAN_Year { get; set; }
        public decimal NCAC718WAMAN_Expenditure { get; set; }
        public string NCAC718WAMANF_Filedesc { get; set; }
        public string NCAC718WAMANF_FileName { get; set; }
        public string NCAC718WAMANF_FilePath { get; set; }
        public bool NCAC718WAMAN_ActiveFlg { get; set; }
        public long NCAC718WAMAN_CreatedBy { get; set; }
        public long NCAC718WAMAN_UpdatedBy { get; set; }
        public DateTime? NCAC718WAMAN_CreatedDate { get; set; }
        public DateTime? NCAC718WAMAN_UpdatedDate { get; set; }
        public long NCAC718WAMANF_Id { get; set; }

        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public long UserId { get; set; }
        public string retrunMsg { get; set; }

        public Array allacademicyear { get; set; }
        public Array alldatalist { get; set; }
        public Array editlisttab1 { get; set; }
        public Array editfilelist { get; set; }
        public NAAC_AC_718_WasteManagement_DTO[] NAACAC7DTO { get; set; }
        public string ASMAY_Year { get; set; }
        public string MI_Name { get; set; }
        public Array institutionlist { get; set; }
        public long NCAC718WAMANC_Id { get; set; }
        public string NCAC718WAMANC_Remarks { get; set; }
        public long? NCAC718WAMANC_RemarksBy { get; set; }
        public string NCAC718WAMANC_StatusFlg { get; set; }
        public bool? NCAC718WAMANC_ActiveFlag { get; set; }
        public long? NCAC718WAMANC_CreatedBy { get; set; }
        public DateTime? NCAC718WAMANC_CreatedDate { get; set; }
        public long? NCAC718WAMANC_UpdatedBy { get; set; }
        public DateTime? NCAC718WAMANC_UpdatedDate { get; set; }
        public Array commentlist { get; set; }
        public string UserName { get; set; }
        public long NCAC718WAMANFC_Id { get; set; }
        public string NCAC718WAMANFC_Remarks { get; set; }
        public long? NCAC718WAMANFC_RemarksBy { get; set; }
        public bool? NCAC718WAMANFC_ActiveFlag { get; set; }
        public long? NCAC718WAMANFC_CreatedBy { get; set; }
        public DateTime? NCAC718WAMANFC_CreatedDate { get; set; }
        public long? NCAC718WAMANFC_UpdatedBy { get; set; }
        public DateTime? NCAC718WAMANFC_UpdatedDate { get; set; }
        public string NCAC718WAMANFC_StatusFlg { get; set; }
        public Array commentlist1 { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
        public string NCAC718WAMAN_StatusFlg { get; set; }
        public string NCAC718WAMANF_StatusFlg { get; set; }
        public bool? NCAC718WAMANF_ActiveFlg { get; set; }
    }
}
