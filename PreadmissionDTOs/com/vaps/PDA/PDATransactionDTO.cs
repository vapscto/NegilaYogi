using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.PDA
{
    public class PDATransactionDTO : CommonParamDTO
    {

        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public long MI_ID { get; set; }
        public long user_id { get; set; }
        public long roleid { get; set; }
        public Array transnumconfig { get; set; }
        public string rolename { get; set; }
        public long PDAE_ID { get; set; }
        public Array receiparraydelete { get; set; }
        public long Amst_Id { get; set; }
        public string AMST_FirstName { get; set; }

        public string AMST_MiddleName { get; set; }

        public string AMST_LastName { get; set; }

        public string PDAE_TransactionNo { get; set; }

        public decimal PDAE_TotAmount { get; set; }
        public string classname { get; set; }
        public string sectionname { get; set; }
        public string AMST_AdmNo { get; set; }
        public DateTime PDAE_Date { get; set; }
        public Array classlist { get; set; }
        public Array pdadata { get; set; }

        public string remarks { get; set; }
        public decimal amount { get; set; }

        public decimal FSS_ToBePaid { get; set; }
        public long FMH_Id { get; set; }
        public PDAExpenditureHeadDTO[] savetmpdata { get; set; }
        public string receiptno { get; set; }
        public string status { get; set; }
        public string transactionno { get; set; }
        public bool returnval { get; set; }
        public bool returnresult { get; set; }
        public string searchType { get; set; }
        public Array searcharray { get; set; }
        public string searchtext { get; set; }
        public DateTime searchdate { get; set; }
        public string searchnumber { get; set; }
        public Array fillyear { get; set; }
        public long FSS_CurrentYrCharges { get; set; }
        public long FSS_PaidAmount { get; set; }

        public long studentdata { get; set; }
        public long refundamt { get; set; }
        public DateTime PDARFD_Date { get; set; }
        public long PDAMH_Id { get; set; }

        public long PDAS_Id { get; set; }
        public decimal PDAS_OBStudentDue { get; set; }

        // public long AMST_Id { get; set; }
        public decimal PDAS_OBExcessPaid { get; set; }
        public decimal PDAS_CYDeposit { get; set; }
        public decimal PDAS_CYExpenses { get; set; }
        public decimal PDAS_CYRefundAmt { get; set; }
        public decimal PDAS_CBStudentDue { get; set; }
        public decimal PDAS_CBExcessPaid { get; set; }

        public DateTime? From_Date { get; set; }

        public DateTime? To_Date { get; set; }
        public Array headwise { get; set; }

        public Array fillsection { get; set; }
        public Array fillstudent { get; set; }

        public long ASMCL_Id { get; set; }
        public long AMSC_Id { get; set; }
        public Array sectionlist { get; set; }
        public Array admsudentslist { get; set; }
        public string ASMC_SectionName { get; set; }
        public string type { get; set; }
        public long[] AMST_Ids { get; set; }
        public long[] ASMCL_Ids { get; set; }
        public DateTime PDAR_ChequeDDDate { get; set; }
        public string PDAR_BankName { get; set; }
        public string PDAR_RefundRemarks { get; set; }
        public string PDAR_ChequeDDNo { get; set; }
        public string PDAR_ModeOfPayment { get; set; }
        public long PDAR_Id { get; set; }
        public int ASMC_order { get; set; }
        public Array fillmonth { get; set; }
        public Array pdaheadlist { get; set; }
        public Array invheadlist { get; set; }
        public long INVMG_Id { get; set; }
        public string INVMG_GroupName { get; set; }
        public long ASMS_Id { get; set; }
        public Array invdetails { get; set; }
        public Array pdadetails { get; set; }
        public Array obdetails { get; set; }

        public Master_NumberingDTO transnumconfigsettings { get; set; }


    }
}
