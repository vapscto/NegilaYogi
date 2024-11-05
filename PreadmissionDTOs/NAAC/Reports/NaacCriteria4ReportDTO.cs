using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Reports
{
    public class NaacCriteria4ReportDTO
    {
        public long NCAC424EXP_Id { get; set; }
        public long MI_Id { get; set; }
        public Array alldata4 { get; set; }
        public Array alldata42 { get; set; }
        public Array alldata5 { get; set; }
        public Array alldata434 { get; set; }
        public Array alldata6 { get; set; }
        public Array alldata62 { get; set; }
        public Array alldata7 { get; set; }
        public Array alldata72 { get; set; }
        public Array alldata8 { get; set; }
        public Nullable<decimal> NCAC424EXP_BooksExp { get; set; }
        public Nullable<decimal> NCAC424EXP_JournalExp { get; set; }
        public Nullable<long> NCAC424EXP_ExpYear { get; set; }
        public Nullable<decimal> NCAC424EXP_EJournalExp { get; set; }
        public string ASMAY_Year { get; set; }
        public Nullable<decimal> NCAC441EXACFC_ExpAccFacility { get; set; }
        public Nullable<decimal> NCAC441EXACFC_ExpPhyFacility { get; set; }
        public Nullable<long> NCAC441EXACFC_Year { get; set; }
        public long NCAC441ExAcFc_Id { get; set; }
        public string msg1 { get; set; }
        public string NCAC423MEM_Membership { get; set; }
        public string NCAC423MEM_Subscription { get; set; }
        public Nullable<long> NCAC423MEM_NoOfEResources { get; set; }
        public Nullable<long> NCAC423MEM_ValidityPeriod { get; set; }
        public string NCAC423MEM_UsageReport { get; set; }
        public Nullable<bool> NCAC423MEM_RemoteAccessFlg { get; set; }
        public string NCAC423MEM_FileName { get; set; }
        public string NCAC423MEM_FilePath { get; set; }
        public long NCAC424EXPF_Id { get; set; }
        public string NCAC424EXPF_FileName { get; set; }
        public string NCAC424EXPF_FilePath { get; set; }
        public string NCAC424EXPF_Filedesc { get; set; }
        public long NCAC423MEM_Id { get; set; }
        public Array yearlist { get; set; }
        public Array alldata82 { get; set; }
        public NaacCriteria4ReportDTO[] selectedYear { get; set; }
        public NaacCriteria4ReportDTO[] selected_Inst { get; set; }
        public long UserId { get; set; }
        public long cycleid { get; set; }
        public Array reportlist { get; set; }
        public Array reportlist2 { get; set; }
        public long ASMAY_Id { get; set; }
        public Array getinstitutioncycle { get; set; }
        public Array getinstitution { get; set; }
    }
}
    