using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
   public class NAAC_AC_7113_CoreValues_DTO : CommonParamDTO
    {
        public long NCAC7113CORVAL_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC7113CORVAL_Year { get; set; }
        public string NCAC7113CORVAL_URL { get; set; }
        public string NCAC7113CORVALF_Filedesc { get; set; }
        public string NCAC7113CORVALF_FileName { get; set; }
        public string NCAC7113CORVALF_FilePath { get; set; }
        public bool NCAC7113CORVAL_ActiveFlg { get; set; }
        public long NCAC7113CORVAL_CreatedBy { get; set; }
        public long NCAC7113CORVAL_UpdatedBy { get; set; }
        public DateTime NCAC7113CORVAL_CreatedDate { get; set; }
        public DateTime NCAC7113CORVAL_UpdatedDate { get; set; }


        public long NCAC7113CORVALF_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long UserId { get; set; }
        public Array allacademicyear { get; set; }
        public Array alldatalist { get; set; }
        public NAAC_AC_7113_CoreValues_DTO[] filelist { get; set; }
        public Array editfilelist { get; set; }
        public Array editlisttab1 { get; set; }
        public Array view { get; set; }
        public string ASMAY_Year { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public string MI_Name { get; set; }
        public Array institutionlist { get; set; }
        public string NCAC7113CORVAL_StatusFlg { get; set; }

        public Array commentlist { get; set; }

        public long NCAC7113CORVALC_Id { get; set; }
        public string NCAC7113CORVALC_Remarks { get; set; }
        public long? NCAC7113CORVALC_RemarksBy { get; set; }
        public string NCAC7113CORVALC_StatusFlg { get; set; }
        public string UserName { get; set; }
        public bool? NCAC7113CORVALC_ActiveFlag { get; set; }
        public long? NCAC7113CORVALC_CreatedBy { get; set; }
        public DateTime? NCAC7113CORVALC_CreatedDate { get; set; }
        public long? NCAC7113CORVALC_UpdatedBy { get; set; }
        public DateTime? NCAC7113CORVALC_UpdatedDate { get; set; }
        public Array commentlist1 { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
        public long NCAC7113CORVALFC_Id { get; set; }
        public string NCAC7113CORVALFC_Remarks { get; set; }
        public long? NCAC7113CORVALFC_RemarksBy { get; set; }
        public bool? NCAC7113CORVALFC_ActiveFlag { get; set; }
        public long? NCAC7113CORVALFC_CreatedBy { get; set; }
        public DateTime? NCAC7113CORVALFC_CreatedDate { get; set; }
        public long? NCAC7113CORVALFC_UpdatedBy { get; set; }
        public DateTime? NCAC7113CORVALFC_UpdatedDate { get; set; }
        public string NCAC7113CORVALFC_StatusFlg { get; set; }
        public string NCAC7113CORVALF_StatusFlg { get; set; }


    }
}
