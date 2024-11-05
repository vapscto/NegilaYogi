using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
   public class NaacBudget_414_DTO
    {
        public long NCAC414BD_Id { get; set; }
        public long MI_Id { get; set; }
        public string NCAC414BD_Budget { get; set; }
        public long NCAC414BD_AllotYear { get; set; }
        public string NCAC414BD_FileName { get; set; }
        public string NCAC414BD_FilePath { get; set; }
        public Nullable<bool> NCAC414BD_ActiveFlg { get; set; }
        public Nullable<long> NCAC414BD_CreatedBy { get; set; }
        public Nullable<long> NCAC414BD_UpdatedBy { get; set; }
        public long UserId { get; set; }
        public Array allacademicyear { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public long IMFY_Id { get; set; }
        public string msg { get; set; }
        public string ASMAY_Year { get; set; }
        public string NCAC414BD_BudgetAllotDev { get; set; }
        public decimal NCAC414BD_BudgetINfDev { get; set; }
        public decimal NCAC414BD_TotalExpExcludeSal { get; set; }
        public decimal NCAC414BD_BudgetINfAugn { get; set; } 

        public long NCAC414BDF_Id { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public NaacBudget_414_DTO[] filelist { get; set; }

        public Array alldata1 { get; set; }
        public Array editlist { get; set; }
        public Array editFileslist { get; set; }
        public Array viewuploadflies { get; set; }
        
        public Array institutionlist { get; set; }
        public string NCAC414BD_StatusFlg { get; set; }
        public string NCAC414BDF_StatusFlg { get; set; }
        public bool NCAC414BDF_ActiveFlg { get; set; }
        public Array commentlist { get; set; }
        public string NCAC414BDC_Remarks { get; set; }
        public long? NCAC414BDC_RemarksBy { get; set; }
        public string NCAC414BDC_StatusFlg { get; set; }
        public bool? NCAC414BDC_ActiveFlag { get; set; }
        public long? NCAC414BDC_CreatedBy { get; set; }
        public DateTime? NCAC414BDC_CreatedDate { get; set; }
        public long? NCAC414BDC_UpdatedBy { get; set; }
        public DateTime? NCAC414BDC_UpdatedDate { get; set; }
        public long NCAC414BDC_Id { get; set; }
        public string UserName { get; set; }
        public Array commentlist1 { get; set; }
        public long NCAC414BDFC_Id { get; set; }
        public string NCAC414BDFC_Remarks { get; set; }
        public long? NCAC414BDFC_RemarksBy { get; set; }
        public bool? NCAC414BDFC_ActiveFlag { get; set; }
        public long? NCAC414BDFC_CreatedBy { get; set; }
        public DateTime? NCAC414BDFC_CreatedDate { get; set; }
        public long? NCAC414BDFC_UpdatedBy { get; set; }
        public DateTime? NCAC414BDFC_UpdatedDate { get; set; }
        public string NCAC414BDFC_StatusFlg { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
    }   
}
