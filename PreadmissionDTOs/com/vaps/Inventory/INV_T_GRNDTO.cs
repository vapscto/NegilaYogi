using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class INV_T_GRNDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string trans_id { get; set; }
        public long INVMI_Id { get; set; }
        public string INVMI_ItemName { get; set; }
        public long INVMUOM_Id { get; set; }
        public string INVMUOM_UOMName { get; set; }
        public string INVMUOM_UOMAliasName { get; set; }

        public long INVMT_Id { get; set; }
        public string INVMT_TaxName { get; set; }
        public string INVMT_TaxAliasName { get; set; }
        public decimal INVMIT_TaxValue { get; set; }
        
        public long INVMGRN_Id { get; set; }
        public long? INVMS_Id { get; set; }
        public string INVMGRN_GRNNo { get; set; }
        public string INVMS_SupplierName { get; set; }
        public string INVMS_StoreName { get; set; }
        public  string  INVMGRN_InvoiceNo { get; set; }
        public DateTime? INVMGRN_PurchaseDate { get; set; }
        public decimal INVMGRN_PurchaseValue { get; set; }
        public decimal? INVMGRN_TotDiscount { get; set; }
        public decimal? INVMGRN_TotTaxAmt { get; set; }
        public decimal INVMGRN_TotalAmount { get; set; }
        public string INVMGRN_Remarks { get; set; }
        public string INVMGRN_ReturnFlg { get; set; }
        public string INVMGRN_PaidFlg { get; set; }
        public bool INVMGRN_CreditFlg { get; set; }
        public bool INVMGRN_ActiveFlg { get; set; }

        public long INVTGRN_Id { get; set; }
        public string INVTGRN_BatchNo { get; set; }
        public decimal INVTGRN_PurchaseRate { get; set; }
        public decimal INVTGRN_MRP { get; set; }
        public decimal INVTGRN_SalesPrice { get; set; }
        public decimal INVTGRN_DiscountAmt { get; set; }
        public decimal INVTGRN_TaxAmt { get; set; }
        public decimal INVTGRN_Amount { get; set; }
        public decimal INVTGRN_Qty { get; set; }
        public string INVTGRN_Naration { get; set; }
        public DateTime? INVTGRN_MfgDate { get; set; }
        public DateTime? INVTGRN_ExpDate { get; set; }
        public bool INVTGRN_ReturnFlg { get; set; }
        public decimal INVTGRN_ReturnQty { get; set; }
        public DateTime? INVTGRN_ReturnDate { get; set; }
        public string INVTGRN_ReturnNo { get; set; }
        public string INVTGRN_ReturnNaration { get; set; }
        public bool INVTGRN_ActiveFlg { get; set; }

        public long INVTGRNT_Id { get; set; }
        public decimal INVTGRNT_TaxPer { get; set; }
        public decimal INVTGRNT_TaxAmt { get; set; }
        public bool INVTGRNT_ActiveFlg { get; set; }

        public long INVMGRNPO_Id { get; set; }
        public long INVMPO_Id { get; set; }

        public long INVMGRNS_Id { get; set; }
        public long INVMST_Id { get; set; }
        public string INVC_LIFOFIFOFlg { get; set; }
        public string SearchColumn { get; set; }
        public string EnteredData { get; set; }
        //=============================Report
        public Array get_grn_item_supplier { get; set; }
        public Array actidactive_flg { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string optionflag { get; set; }
        public string typeflag { get; set; }
        public string bwdateflag { get; set; }
        public Array get_grnreport { get; set; }
        public Array year_list_ob { get; set; }
        public Array store_list_ob { get; set; }


        //==================================
        public Array get_item { get; set; }
        public Array get_Store { get; set; }
        public Array get_supplier { get; set; }
        public Array get_tax { get; set; }

        public Array get_itemDetail { get; set; }
        public Array get_itemTax { get; set; }
        public Array get_GRN { get; set; }
        public Array get_GRNItemDetails { get; set; }
        public Array get_GRNItemTax { get; set; }
        public Array edit_GRN_Details_List { get; set; }
        public Array edit_GRN_Master_Details { get; set; }
        public GRNItemDTO[] GRNItem { get; set; }
        public grnArrayDTO[] grnArray { get; set; }
        public itemArrayDTO[] itemArray { get; set; }
        public supplierArrayDTO[] supplierArray { get; set; }

        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }

    }
    public class GRNItemDTO
    {
        public long INVMGRN_Id { get; set; }
        public long INVMI_Id { get; set; }
       // public long? INVMI_Id1 { get; set; }
        public long INVMUOM_Id { get; set; }
        public string INVTGRN_BatchNo { get; set; }
        public decimal INVTGRN_PurchaseRate { get; set; }
        public decimal INVTGRN_MRP { get; set; }
        public decimal INVTGRN_SalesPrice { get; set; }
        public decimal INVTGRN_DiscountAmt { get; set; }
        public decimal INVTGRN_TaxAmt { get; set; }
        public decimal INVTGRN_Amount { get; set; }
        public decimal INVTGRN_Qty { get; set; }
        public string INVTGRN_Naration { get; set; }
        public DateTime? INVTGRN_MfgDate { get; set; }
        public DateTime? INVTGRN_ExpDate { get; set; }
        public bool INVTGRN_ReturnFlg { get; set; }
        public decimal INVTGRN_ReturnQty { get; set; }
        public DateTime? INVTGRN_ReturnDate { get; set; }
        public string INVTGRN_ReturnNo { get; set; }
        public string INVTGRN_ReturnNaration { get; set; }
        public bool INVTGRN_ActiveFlg { get; set; }
        public Array get_itemTax { get; set; }

        public GRNItemTaxDTO[] GRNItemTax { get; set; }
    }
    public class GRNItemTaxDTO
    {
        public long INVMT_Id { get; set; }
        public decimal INVTGRNT_TaxPer { get; set; }
        public decimal INVTGRNT_TaxAmt { get; set; }
        public bool INVTGRNT_ActiveFlg { get; set; }

    }

    public class grnArrayDTO
    {
        public long INVMGRN_Id { get; set; }
        public string INVMGRN_GRNNo { get; set; }

    }
    public class itemArrayDTO
    {
        public long INVMI_Id { get; set; }   
    }
    public class supplierArrayDTO
    {
        public long INVMS_Id { get; set; }       

    }
}
