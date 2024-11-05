using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class NaacExpenditure424DTO
    {

        public long NCAC424EXP_Id { get; set; }
        public long MI_Id { get; set; }
        public Nullable<decimal> NCAC424EXP_BooksExp { get; set; }
        public Nullable<decimal> NCAC424EXP_JournalExp { get; set; }
        public long NCAC424EXP_ExpYear { get; set; }
        public Nullable<decimal> NCAC424EXP_EJournalExp { get; set; }
        public string NCAC424EXP_FileName { get; set; }
        public string NCAC424EXP_FilePath { get; set; }
        public Nullable<bool> NCAC424EXP_ActiveFlg { get; set; }
        public Nullable<long> NCAC424EXP_CreatedBy { get; set; }
        public Nullable<long> NCAC424EXP_UpdatedBy { get; set; }
        public Nullable<System.DateTime> NCAC424EXP_CreatedDate { get; set; }
        public Nullable<System.DateTime> NCAC424EXP_UpdatedDate { get; set; }
        public long UserId { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public string msg { get; set; }
        public Array alldata1 { get; set; }
        public Array editlist { get; set; }
        public Array reportdata { get; set; }

        public long NCAC424EXPF_Id { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public Array editFileslist { get; set; }
        public Array viewuploadflies { get; set; }
        public NaacExpenditure424DTO[] filelist { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public Array allacademicyear { get; set; }
        public Array institutionlist { get; set; }
        public string NCAC424EXP_StatusFlg { get; set; }
        public string NCAC424EXPF_StatusFlg { get; set; }
        public bool NCAC424EXPF_ActiveFlg { get; set; }
        public long NCAC424EXPC_Id { get; set; }
        public string NCAC424EXPC_Remarks { get; set; }
        public long? NCAC424EXPC_RemarksBy { get; set; }
        public string NCAC424EXPC_StatusFlg { get; set; }
        public bool? NCAC424EXPC_ActiveFlag { get; set; }
        public long? NCAC424EXPC_CreatedBy { get; set; }
        public DateTime? NCAC424EXPC_CreatedDate { get; set; }
        public long? NCAC424EXPC_UpdatedBy { get; set; }
        public DateTime? NCAC424EXPC_UpdatedDate { get; set; }
        public long NCAC424EXPFC_Id { get; set; }
        public string NCAC424EXPFC_Remarks { get; set; }
        public long? NCAC424EXPFC_RemarksBy { get; set; }
        public bool? NCAC424EXPFC_ActiveFlag { get; set; }
        public long? NCAC424EXPFC_CreatedBy { get; set; }
        public DateTime? NCAC424EXPFC_CreatedDate { get; set; }
        public long? NCAC424EXPFC_UpdatedBy { get; set; }
        public DateTime? NCAC424EXPFC_UpdatedDate { get; set; }
        public string NCAC424EXPFC_StatusFlg { get; set; }
        public Array commentlist { get; set; }
        public Array commentlist1 { get; set; }
        public string UserName { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }

    }
}
