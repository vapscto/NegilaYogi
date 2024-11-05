using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
   public class NAAC_AC_7114_HumanValues_DTO : CommonParamDTO
    {
        public long NCAC7114HUVAL_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC7114HUVAL_Year { get; set; }
        public string NCAC7114HUVAL_ProgramTitle { get; set; }
        public long NCAC7114HUVAL_NoOfPartcipants { get; set; }
        public DateTime NCAC7114HUVAL_FromDate { get; set; }
        public DateTime NCAC7114HUVAL_ToDate { get; set; }
        public string NCAC7114HUVALF_Filedesc { get; set; }
        public string NCAC7114HUVALF_FileName { get; set; }
        public string NCAC7114HUVALF_FilePath { get; set; }
        public bool NCAC7114HUVAL_ActiveFlg { get; set; }
        public long NCAC7114HUVAL_CreatedBy { get; set; }
        public long NCAC7114HUVAL_UpdatedBy { get; set; }
        public DateTime NCAC7114HUVAL_CreatedDate { get; set; }
        public DateTime NCAC7114HUVAL_UpdatedDate { get; set; }
        public long NCAC7114HUVALF_Id { get; set; }

        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public long UserId { get; set; }
        public string retrunMsg { get; set; }

        public Array allacademicyear { get; set; }
        public Array alldatalist { get; set; }
        public Array editlisttab1 { get; set; }
        public Array editfilelist { get; set; }
        public NAAC_AC_7114_HumanValues_DTO[] NAACAC7114DTO { get; set; }
        public string ASMAY_Year { get; set; }
        public string MI_Name { get; set; }
        public Array institutionlist { get; set; }
        public Array commentlist { get; set; }
        public Array commentlist1 { get; set; }
        public string NCAC7114HUVALC_Remarks { get; set; }
        public long NCAC7114HUVALC_RemarksBy { get; set; }
        public string NCAC7114HUVALC_StatusFlg { get; set; }
        public bool NCAC7114HUVALC_ActiveFlag { get; set; }
        public long NCAC7114HUVALC_CreatedBy { get; set; }
        public DateTime? NCAC7114HUVALC_CreatedDate { get; set; }
        public long NCAC7114HUVALC_UpdatedBy { get; set; }
        public DateTime? NCAC7114HUVALC_UpdatedDate { get; set; }
        public string UserName { get; set; }
        public DateTime? NCAC7114HUVALFC_UpdatedDate { get; set; }
        public string NCAC7114HUVALFC_Remarks { get; set; }
        public long NCAC7114HUVALFC_RemarksBy { get; set; }
        public long NCAC7114HUVALFC_Id { get; set; }
        public string NCAC7114HUVALFC_StatusFlg { get; set; }
        public bool NCAC7114HUVALFC_ActiveFlag { get; set; }
        public long NCAC7114HUVALFC_CreatedBy { get; set; }
        public DateTime? NCAC7114HUVALFC_CreatedDate { get; set; }
        public long NCAC7114HUVALFC_UpdatedBy { get; set; }
        public long filefkid { get; set; }
        public string Remarks { get; set; }
        public string NCAC7114HUVAL_StatusFlg { get; set; }
    }
}
