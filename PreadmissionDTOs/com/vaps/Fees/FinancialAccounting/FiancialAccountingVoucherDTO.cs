using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees.FinancialAccounting
{
   public class FiancialAccountingVoucherDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public string returnval { get; set; }
        public Array fyear { get; set; }
        public Array companyname { get; set; }
        public Array ledgerdetails { get; set; }
        public long FAMVOU_Id { get; set; }
        public long FAMCOMP_Id { get; set; }
        public long FAMLED_Id { get; set; }
        public long IMFY_Id { get; set; }
        public string FAMVOU_VoucherType { get; set; }
        public string FAMVOU_VoucherNo { get; set; }
        public DateTime? FAMVOU_VoucherDate { get; set; }
        public string FAMVOU_Narration { get; set; }
        public string FAMVOU_Suffix { get; set; }
        public string FAMVOU_Prefix { get; set; }
        //public string FAMVOU_VNo { get; set; }     
        public string FAMVOU_UserVoucherType { get; set; }
        public string FAMVOU_APIReferenceNo { get; set; }
        public bool FAMVOU_BillwiseFlg { get; set; }
        public string FAMVOU_Description { get; set; }
        public string FAMLED_LedgerName { get; set; }
        public FA_TVoucherDTO[] FA_T_Voucher { get; set; }
        public Array getreport { get; set; }
        public string FAMCOMP_CompanyName { get; set; }
        public string IMFY_FinancialYear { get; set; }
        public bool FAMVOU_ActiveFlg { get; set; }
        public Array editMvoucher { get; set; }
        public Array editTvoucher { get; set; }
        public string FAMLEDD_OBCRDRFlg { get; set; }
        public Voucher_NumberingDTO[] vouchernumbering { get; set; }
        //public  Voucher_NumberingDTO transnumconfigsettings { get; set; }
    }
    
}
