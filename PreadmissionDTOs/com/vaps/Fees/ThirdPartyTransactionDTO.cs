using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Fee
{
    public class ThirdPartyTransactionDTO
    {


        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public string AMST_AdmNo { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public string ASMCL_ClassName { get; set; }
        public long AMAY_RollNo { get; set; }
        public string ASMC_SectionName { get; set; }
        public string ASMAY_Year { get; set; }
        public bool returnval { get; set; }
        public bool AMST_ActiveFlag { get; set; }
        public long FMH_Id { get; set; }
        public string FYPTP_Towards { get; set; }
        public string FMH_FeeName { get; set; }
        public long FMG_Id { get; set; }
        public string FMG_GroupName { get; set; }
        public string FYGHM_ActiveFlag { get; set; }
        public bool FMG_ActiceFlag { get; set; }

        public int FMH_Order { get; set; }
        public bool FMH_ActiveFlag { get; set; }
        public Array studntlist { get; set; }
        public Array yearlist { get; set; }
        public Array grouplist { get; set; }
        public Array studinfo { get; set; }
        public Array feegrouplist { get; set; }
        public long FYP_Id { get; set; }
        public string FYP_Receipt_No { get; set; }
        public long FYPTP_Id { get; set; }
        public string FYP_Bank_Name { get; set; }
        public string FYP_DD_Cheque_No { get; set; }
        public long user_id { get; set; }
        public string FYP_Bank_Or_Cash { get; set; }

        public DateTime FYP_DD_Cheque_Date { get; set; }
        public DateTime FYP_Date { get; set; }
        public decimal FYP_Tot_Amount { get; set; }
        public string FYP_Remarks { get; set; }
        public string FYPTP_Name { get; set; }
        public Array stdetails { get; set; }
        public Array alldata { get; set; }
        
        public Array EditOther{get;set;}
        public decimal FTP_TotalPaidAmount { get; set; }
        public string TypeSave { get; set; }

        public bool dulicate { get; set; }

        public Array thirdparty_auto_receipt { get; set; }
    }

}
