using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeMasterRefundDTO
    {
        public long FR_ID { get; set; }
        public long FMH_ID { get; set; }
        public long AMST_ID { get; set; }
        public DateTime? FR_Date { get; set; }
        public decimal FR_RefundAmount { get; set; }
        public string FR_RefundFlag { get; set; }
        public long ASMAY_ID { get; set; }
        public string FR_RefundRemarks { get; set; }
        public string FR_RefundNo { get; set; }
        public string FR_BANK_CASH { get; set; }
        public string FR_Favor { get; set; }
        public bool FR_BC_Flag { get; set; }
        public DateTime? FR_CheqDate { get; set; }
        public long FR_CheqNo { get; set; }
        public long FMG_Id { get; set; }
        public long roleid { get; set; }

        public long MI_Id { get; set; }
        public string FR_BankName { get; set; }

        public long FTI_Id { get; set; }
        public string FTI_Name { get; set; }
        
        public long ASMCL_ID { get; set; }
        public long ASMS_Id { get; set; }
        public long FSS_RunningExcessAmount { get; set; }
        public long FSS_Id { get; set; }
        public bool FSS_RefundOverFlag { get; set; }
        public FeeMasterRefundDTO[] savetmpdata { get; set; }
        public FeeMasterRefundDTO[] selectedRefundOverList { get; set; }

        public Array fillyear { get; set; }
        public Array fillclass { get; set; }
        public Array fillsection { get; set; }
        public Array fillgroup { get; set; }
        public Array fillstudent { get; set; }
        public Array fillacclst { get; set; }
        public Array editFeeRefund { get; set; }

        public Array fillthirdgriddata { get; set; }
        public Array masterinstitution { get; set; }
        public Array fillstudentviewdetails { get; set; }
        public Array student_details { get; set; }

        public string FMH_FeeName { get; set; }

        public string filterdata { get; set; }

       // public string validationvalue { get; set; }

        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public string AMST_FatherName { get; set; }
        public string AMST_MotherName { get; set; }
        public string ASMAY_Year { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string AMST_AdmNo { get; set; }
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
        public long FSS_RefundAmount { get; set; }
        public string filterrefund { get; set; }
        public long FSS_RefundableAmount { get; set; }
        public Array showstaticticsdetails { get; set; }
        public string multiplegroupF { get; set; }

        public string FMT_Name { get; set; }
        public int? FMT_Order { get; set; }

        public string instantrefund { get; set; }
        public long[] terms_groups { get; set; }

    }
}
