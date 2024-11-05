using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Fees
{
    public class CLGFeeRefundableDTO
    {
        public long FCR_Id { get; set; }
        public long FMH_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long FCMAS_Id { get; set; }
        public DateTime? FCR_Date { get; set; }
        public decimal FCR_RefundAmount { get; set; }
        public string FR_RefundFlag { get; set; }        
        public string FR_RefundRemarks { get; set; }
        public string FCR_RefundNo { get; set; }
        public string FCR_ModeOfPayment { get; set; }
        public string FCR_RefundRemarks { get; set; }
        public bool FCR_RefundFlag { get; set; }
        public DateTime? FCR_ChequeDDDate { get; set; }
        public long FCR_ChequeDDNo { get; set; }
        public long FMG_Id { get; set; }
        public string FMG_GroupName { get; set; }
        public long MI_Id { get; set; }
        public string FCR_Bank { get; set; }
        public long FCR_OPReferenceNo { get; set; }

        public long FTI_Id { get; set; }
        public string FTI_Name { get; set; }
        
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long FCSS_RunningExcessAmount { get; set; }
        public long FCSS_Id { get; set; }
        public bool FSS_RefundOverFlag { get; set; }
        public CLGFeeRefundableDTO[] savetmpdata { get; set; }
        public CLGFeeRefundableDTO[] selectedRefundOverList { get; set; }

        public Array fillyear { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array semisterlist { get; set; }
        public Array sectionlist { get; set; }
        public Array fillgroup { get; set; }
        public Array studentlist { get; set; }
        public Array fillacclst { get; set; }
        public Array editFeeRefund { get; set; }

        public Array fillthirdgriddata { get; set; }

        public string FMH_FeeName { get; set; }

        public string filterdata { get; set; }

       // public string validationvalue { get; set; }

        public string AMCST_FirstName { get; set; }
        public string AMCST_MiddleName { get; set; }
        public string AMCST_LastName { get; set; }
        public Array currentYear { get; set; }
        public int count { get; set; }
    
        public bool returnVal { get; set; }
        public string returntxt { get; set; }
        
        public Array fillhead  { get; set; }
        public string searchType { get; set; }
        public string searchtext { get; set; }
         public string searchnumber { get; set; }
        public DateTime searchdate { get; set; }

        public string IMN_AutoManualFlag { get; set; }

        public long userid { get; set; }

        public long FSS_PaidAmount { get; set; }
        public long FCSS_RefundAmount { get; set; }
        public string filterrefund { get; set; }
        public long FCSS_RefundableAmount { get; set; }
        public Array showstaticticsdetails { get; set; }
        public string multiplegroupF { get; set; }
        public long FCSS_ToBePaid { get; set; }
    }
}
