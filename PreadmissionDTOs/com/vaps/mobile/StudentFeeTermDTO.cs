using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.mobile
{
    public class StudentFeeTermDTO
    {
        public class input
        {
            public long AMST_Id { get; set; }
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
        }
        public class customgroup
        {
            //  public long FMT_Id { get; set; }
            //  public string FMT_Name { get; set; }
            public long FMGG_Id { get; set; }
            public string FMGG_GroupName { get; set; }
            public long C_FSS_TotalToBePaid { get; set; }
            public long C_FSS_ToBePaid { get; set; }
            public long C_FSS_PaidAmount { get; set; }
            public long C_FSS_ConcessionAmount { get; set; }
            public long C_FSS_FineAmount { get; set; }
            public Array feetermorgroup { get; set; }
        }
        public class TermorGroup
        {
            public string FMT_Name { get; set; }         
            public long FSS_TotalToBePaid { get; set; }
            public long FSS_ToBePaid { get; set; }
            public long FSS_PaidAmount { get; set; }
            public long FSS_ConcessionAmount { get; set; }
            public long FSS_FineAmount { get; set; }
        }
        // public class PaymenthistoryDTO
        //{
        //    public string Terms { get; set; }
        //    public Decimal Paid_Amt { get; set; }
        //    public DateTime Date { get; set; }
        //    public string Bank_Or_Cash { get; set; }
        //    public string Receipt_No { get; set; }
        //    public string Bank_Name { get; set; }
        //    public long DD_Cheque_No { get; set; }
        //}
        public long Total_FSS_TotalToBePaid { get; set; }
        public long Total_FSS_ToBePaid { get; set; }
        public decimal Total_FSS_PaidAmount { get; set; }
        public long Total_FSS_ConcessionAmount { get; set; }
        public long Total_FSS_FineAmount { get; set; }
        public Array customgroup_array { get; set; }
        public Array paymenthistory { get; set; }
    }
}
