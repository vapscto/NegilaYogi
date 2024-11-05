using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
   public class NAAC_AC_7116_StatutoryBodies_DTO : CommonParamDTO
    {
        public long NCAC7116STABOD_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC7116STABOD_Year { get; set; }
        public string NCAC7116STABOD_URL { get; set; }
        public string NCAC7116STABODF_Filedesc { get; set; }
        public string NCAC7116STABODF_FileName { get; set; }
        public string NCAC7116STABODF_FilePath { get; set; }
        public bool NCAC7116STABOD_ActiveFlg { get; set; }
        public long NCAC7116STABOD_CreatedBy { get; set; }
        public long NCAC7116STABOD_UpdatedBy { get; set; }
        public DateTime NCAC7116STABOD_CreatedDate { get; set; }
        public DateTime NCAC7116STABOD_UpdatedDate { get; set; }
        public long NCAC7116STABODF_Id { get; set; }

        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public long UserId { get; set; }
        public string retrunMsg { get; set; }

        public Array allacademicyear { get; set; }
        public Array alldatalist { get; set; }
        public Array editlisttab1 { get; set; }
        public Array editfilelist { get; set; }
        public NAAC_AC_7116_StatutoryBodies_DTO[] NAACAC7116DTO { get; set; }
        public string ASMAY_Year { get; set; }
        public string MI_Name { get; set; }
        public Array institutionlist { get; set; }
        public long NCAC7116STABODC_Id { get; set; }
        public string NCAC7116STABODC_Remarks { get; set; }
        public long? NCAC7116STABODC_RemarksBy { get; set; }
        public string NCAC7116STABODC_StatusFlg { get; set; }
        public bool? NCAC7116STABODC_ActiveFlag { get; set; }
        public long? NCAC7116STABODC_CreatedBy { get; set; }
        public DateTime? NCAC7116STABODC_CreatedDate { get; set; }
        public long? NCAC7116STABODC_UpdatedBy { get; set; }
        public DateTime? NCAC7116STABODC_UpdatedDate { get; set; }
        public long NCAC7116STABODFC_Id { get; set; }
        public string NCAC7116STABODFC_Remarks { get; set; }
        public long? NCAC7116STABODFC_RemarksBy { get; set; }
        public bool? NCAC7116STABODFC_ActiveFlag { get; set; }
        public long? NCAC7116STABODFC_CreatedBy { get; set; }
        public DateTime? NCAC7116STABODFC_CreatedDate { get; set; }
        public long? NCAC7116STABODFC_UpdatedBy { get; set; }
        public DateTime? NCAC7116STABODFC_UpdatedDate { get; set; }
        public string NCAC7116STABODFC_StatusFlg { get; set; }
        public string UserName { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
        public Array commentlist { get; set; }
        public Array commentlist1 { get; set; }
        public string NCAC7116STABOD_StatusFlg { get; set; }
        public string NCAC7116STABODF_StatusFlg { get; set; }
        public bool? NCAC7116STABODF_ActiveFlg { get; set; }
    }
}
