using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Canteen
{
    public class FoodtransactionDTO
    {
        public long MI_Id { get; set; }
        public long CMMFIT_Id { get; set; }
        public long CMMFI_Id { get; set; }
        public long INVMT_Id { get; set; }
        public long CMMCA_Id { get; set; }
        public long PDAS_Id { get; set; }
        public long ASMAY_Id { get; set; }

        
        public long AMST_Id { get; set; }

        public string AMCTST_IP { get; set; }
        public decimal CMMFIT_TaxPercent { get; set; }
        public bool CMMFIT_ActiveFlg { get; set; }
        public long CMMFIT_CreatedBy { get; set; }
        public long CMMFIT_UpdatedBy { get; set; }
        public DateTime CMMFIT_CreatedDate { get; set; }
        public Array foodtax { get; set; }
        public Array Foodcategeory { get; set; }
        public Array Fooditeam { get; set; }
        public Array get_foodtaxDetail { get; set; }
        public Array invmaster { get; set; }
        public Array get_foodDetail { get; set; }
        public Array modeofpayment { get; set; }
        public Array Transactiondeatils { get; set; }

        public Array getstudentdetails { get; set; }
        public Array Payment_deatils { get; set; }

        public string School_Flag { get; set; }

        public Array order_deatils { get; set; }
        public Array getstudentdetails_cancel { get; set; }
        public string returnval { get; set; }
        public long UserId { get; set; }
        public string CMMFI_FoodItemName { get; set; }
        public string CMMCA_CategoryName { get; set; }
        public string INVMT_TaxName { get; set; }
        public string staticPart { get; set; }
        public string dynamicPart { get; set; }
        public string orderID { get; set; }
        public decimal taxpercent { get; set; }
        public decimal CMMFI_UnitRate { get; set; }
        public bool CMMFI_OutofStockFlg { get; set; }
        public bool CMMFI_ActiveFlg { get; set; }
        public bool CMMCA_ActiveFlag { get; set; }
        public string Transcationnum { get; set; }
        public long CM_orderID { get; set; }
        public long CMTRANS_Id { get; set; }
        public long ACMST_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool CMTRANS_MemberFlg { get; set; }
        public decimal CMTRANS_Amount { get; set; }
        public decimal PDAS_CYExpenses { get; set; }
        public decimal CMTRANS_TaxAmount { get; set; }
        public decimal CMTRANS_TotalAmount { get; set; }
        public string CMTRANS_Remarks { get; set; }
        public decimal CMTRANS_PaidAmount { get; set; }
        public decimal CMTRANS_PendingAmount { get; set; }
        public bool CMTRANS_KOTPrintedFlg { get; set; }
        public string CMTRANS_NoofKOTPrints { get; set; }
        public bool CMTRANS_VoidKotFlg { get; set; }
        public string CMTRANS_VoidReasons { get; set; }
        public string displaymessage { get; set; }
        public bool CMTRANS_SelfCheckInFlg { get; set; }

        public string Flag { get; set; }
        public string Fromdate { get; set; }
        public string Todate { get; set; }
        public string ItemName { get; set; }
        public string Category { get; set; }

        public Array Month_Daywise_deatils { get; set; }
        public Array YearWise_deatils { get; set; }
        public Array food_OrderDetails { get; set; }
        public Array Payment_deatils_print { get; set; }



        public int ? CMTRANS_SecurityCode { get; set; }
        public Array paymenttrans { get; set; }

        public CMTransactionTaxDTO[] CMTransactionTax { get; set; }
        public CMTransactionPaymentModeDTO[] CMTransactionPaymentMode { get; set; }
        public CMTransactionItemsDTO[] CMTransactionItems { get; set; }

    }

    public class CMTransactionTaxDTO
    {

        public long CMTRANST_Id { get; set; }
        public long CMTRANS_Id { get; set; }
        public long INVMT_Id { get; set; }
        public long CMTRANST_TaxAmount { get; set; }

    }

    public class CMTransactionItemsDTO
    {
        public long CMTRANSI_Id { get; set; }
        public long CMTRANS_Id { get; set; }
        public string CMTRANSI_name { get; set; }
        public long cmmfI_Id { get; set; }
        public decimal itemCount { get; set; }
        public decimal unitRate { get; set; }

    
    }

    public class CMTransactionPaymentModeDTO
    {

        public long CMTRANSPM_Id { get; set; }
        public long CMTRANS_Id { get; set; }
        public int CMTRANSPM_PaymentModeId { get; set; }
        public string CMTRANSPM_PaymentMode { get; set; }
    }


}
