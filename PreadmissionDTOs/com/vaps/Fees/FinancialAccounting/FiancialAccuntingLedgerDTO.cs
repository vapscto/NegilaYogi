using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees.FinancialAccounting
{
    public class FiancialAccuntingLedgerDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public string returnval {get; set;}
        public long FAMLED_Id { get; set; }
        public long FAMCOMP_Id { get; set; }
        public long IMFY_Id { get; set; }
        public long FAMGRP_Id { get; set; }
        public long FAUGRP_Id { get; set; }
        public string FAMLED_LedgerName { get; set; }
        public string FAMLED_LedgerAliasName { get; set; }
        public DateTime? FAMLED_LedgerCreatedDate { get; set; }
        public string FAMLED_Remarks { get; set; }
        public string FAMLED_PostalAddress { get; set; }
        public string FAMLED_EmailAddress { get; set; }
        public string FAMLED_Type { get; set; }
        public string FAMLED_Under { get; set; }
        public bool FAMLED_BillwiseFlg { get; set; }
        public bool FAMLED_ActiveFlg { get; set; }
        public Array getreport { get; set; }
        public Array getgroupname { get; set; }
        public Array companyname { get; set; }
        public Array fyear { get; set; }
        public Array usergroupname { get; set; }
        public string FAUGRP_UserGroupName { get; set; }
        public faledgerdetails[] ledgerdetails { get; set; }
        public Array editledger { get; set; }
        public Array editledgerdetail { get; set; }


        //FA_Ledger_details
        public decimal FAMLEDD_OpeningBalance { get; set; }
        public string FAMLEDD_OBCRDRFlg { get; set; }
        public decimal FAMLEDD_ClosingBalance { get; set; }
        public string FAMLEDD_CBCRDRFlg { get; set; }
        public DateTime? FAMLEDD_OBDate { get; set; }
        public decimal FAMLEDD_BudgetAmount { get; set; }
        public string FAMLEDD_Remarks { get; set; }
    }
    public class faledgerdetails
    {
        public decimal FAMLEDD_OpeningBalance { get; set; }
        public string FAMLEDD_OBCRDRFlg { get; set; }
        public decimal FAMLEDD_ClosingBalance { get; set; }
        public string FAMLEDD_CBCRDRFlg { get; set; }
        public DateTime? FAMLEDD_OBDate { get; set; }
        public decimal FAMLEDD_BudgetAmount { get; set; }
        public string FAMLEDD_Remarks { get; set; }
        public long? FAMLED_Id { get; set; }


    }
}
