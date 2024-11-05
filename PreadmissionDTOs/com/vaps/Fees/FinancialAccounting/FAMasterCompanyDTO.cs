using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees.FinancialAccounting
{
   public class FAMasterCompanyDTO
    {

        public long FAMCOMP_Id { get; set; }
        public long MI_Id { get; set; }
        public long user_id { get; set; }
        public string FAMCOMP_CompanyName { get; set; }
        public string FAMCOMP_Description { get; set; }

        public string FAMCOMP_CompanyAddress { get; set; }
        public string FAMCOMP_EMailId { get; set; }
        public string FAMCOMP_PhoneNo { get; set; }
        public string FAMCOMP_IncomeTaxNo { get; set; }
        public string FAMCOMP_SalesTaxNo { get; set; }
       

        public DateTime? FAMCOMP_UpdatedDate { get; set; }

        public DateTime? FAMCOMP_CreatedDate { get; set; }

        public string FAMCOMP_Password { get; set; }
        public DateTime? FAMCOMP_BookBeginingDate { get; set; }
        public bool FAMCOMP_PrintReceiptFlg { get; set; }
        public bool FAMCOMP_DuplicateVoucherFlg { get; set; }
        public bool FAMCOMP_UseBillWiseDetailsFlg { get; set; }

        public bool FAMCOMP_UseDebitCreditFlg { get; set; }
        public bool FAMCOMP_CMPTypeFlg { get; set; }
       
      

        public bool FAMCOMP_SetTypeFlg { get; set; }
        public bool FAMCOMP_SetLedgerBalanceFlg { get; set; }
        public bool FAMCOMP_SetDispFlg { get; set; }
        public bool FAMCOMP_SetNegBalanceFlg { get; set; }
        public string FAMCOMP_StatusFlg { get; set; }
        public bool FAMCOMP_ActiveFlg { get; set; }

        public string returnval { get; set; }
        
        public Array masterCompanyDetails { get; set; }

        public long FAMCOMP_CreatedBy { get; set; }
        public long FAMCOMP_UpadatedBY { get; set; }

        //
       


    }
}
