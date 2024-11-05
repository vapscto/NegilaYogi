using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees.FinancialAccounting
{
   public class FA_TVoucherDTO
    {
        public long FATVOU_Id { get; set; }
        public long FAMVOU_Id { get; set; }
        public long FAMLED_Id { get; set; }
        public decimal? FATVOU_Amount { get; set; }
        public string FATVOU_CRDRFlg { get; set; }
        public string FATVOU_TransactionTypeFlg { get; set; }
        public string FATVOU_Narration { get; set; }
        public string FATVOU_ChequNo { get; set; }
        public DateTime? FATVOU_ChequeDate { get; set; }
        public string FATVOU_BankName { get; set; }
        public string FATVOU_ReferrenceNo { get; set; }
        public bool FATVOU_BillwiseFlg { get; set; }
        public string FATVOU_Description { get; set; }
       
       

    }
}
