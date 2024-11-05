using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Purchase.Inventory
{
    public class INV_VendorPaymentDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public string optionflag { get; set; }
        public long UserId { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string trans_id { get; set; }
        public long INVPITS_Id { get; set; }
        public long INVMS_Id { get; set; }
        public long INVMPI_Id { get; set; }
        public string INVMS_SupplierName { get; set; }
        public long INVSPT_Id { get; set; }
        public DateTime INVSPT_PaymentDate { get; set; }
        public string INVSPT_ModeOfPayment { get; set; }
        public string INVSPT_PaymentReference { get; set; }
        public string INVSPT_ChequeDDNo { get; set; }
        public string INVSPT_BankName { get; set; }
        public DateTime? INVSPT_ChequeDDDate { get; set; }
        public decimal INVSPT_Amount { get; set; }
        public string INVSPT_Remarks { get; set; }
        public bool INVSPT_ActiveFlg { get; set; }
        public long INVSPT_CreatedBy { get; set; }
        public long INVSPT_UpdatedBy { get; set; }

        public long INVSPTGRN_Id { get; set; }
        public long INVMGRN_Id { get; set; }
        public string INVMGRN_GRNNo { get; set; }
        public decimal INVMGRN_PurchaseValue { get; set; }
        public decimal INVSPTGRN_Amount { get; set; }    
        public string INVSPTGRN_Remarks { get; set; }
        public bool INVSPTGRN_ActiveFlg { get; set; }
        public long INVSPTGRN_CreatedBy { get; set; }
        public long INVSPTGRN_UpdatedBy { get; set; }
        public decimal INVMGRN_TotalPaid { get; set; }
        public decimal INVMGRN_TotalBalance { get; set; }
        public string paymenttype { get; set; }


        public Array get_supplier { get; set; }
        public Array get_SuplierGRNno { get; set; }
        public Array get_GRNpayment { get; set; }
        public Array get_paymentMode { get; set; }
        public Array get_vendorpayment { get; set; }
        public Array get_modeldetails { get; set; }
        public Array get_VPReport_Details { get; set; }
        public Array get_VPreport { get; set; }
   
        public paymentArrayDTO[] paymentArray { get; set; }
    }
    public class paymentArrayDTO
    {
        public long INVSPTGRN_Id { get; set; }
        public long INVMGRN_Id { get; set; }
        public decimal INVSPTGRN_Amount { get; set; }
        public string INVSPTGRN_Remarks { get; set; }
        public decimal INVMGRN_TotalPaid { get; set; }
        public decimal INVMGRN_TotalBalance { get; set; }

    }

}
