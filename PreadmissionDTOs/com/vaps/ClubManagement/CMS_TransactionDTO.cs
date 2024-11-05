using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.ClubManagement
{
   public class CMS_TransactionDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public string returnval { get; set; }
        public Array getreport { get; set; }
        public long CMSTRANS_Id { get; set; }
        public long CMSMMEM_Id { get; set; }
        public long IMFY_Id { get; set; }
        public  Array finacial { get; set; }
        public string CMSTRANS_TransactionNo { get; set; }
        public DateTime? CMSTRANS_Date { get; set; }
        public decimal CMSTRANS_TotalAmount { get; set; }
        public decimal CMSTRANS_TotalTax { get; set; }
        public decimal CMSTRANS_TotalNetAmount { get; set; }
        public bool CMSTRANS_CreditTransFlg { get; set; }
        public long CMSTRANS_NoOFGuests { get; set; }
        public string CMSTRANS_GuestsName { get; set; }
        public string CMSTRANS_GuestContactNo { get; set; }
        public string CMSTRANS_Remarks { get; set; }
        public bool CMSTRANS_ActiveFlg { get; set; }
        public Array memberarray { get; set; }
        public string membername { get; set; }
        public Array getname { get; set; }
        public string IMFY_FinancialYear { get; set; }
        public TransactionMember[] TransctionNon_Member { get; set; }
       public Array editnmember { get; set; }
        public Array edittransction { get; set; }
    }
    public class TransactionMember
    {
        public string CMSTRANSNMEM_NonMemberName { get; set; }
        public string CMSTRANSNMEM_ContactNo { get; set; }
        public string CMSTRANSNMEM_EmailId { get; set; }
        public string CMSTRANSNMEM_Address { get; set; }
    }
}
