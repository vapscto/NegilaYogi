using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class INV_T_SalesReturnDTO
    {
        public long MI_Id { get; set; }
        public long INVMSL_Id { get; set; }
        public long INVMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long UserId { get; set; }
        public long INVTSLRET_Id { get; set; }
        public long INVMSLRET_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMT_Id { get; set; }
        public long INVMUOM_Id { get; set; }
        public long INVMP_Id { get; set; }
        public string INVMI_ItemName { get; set; }
        public string INVMT_TaxName { get; set; }
        public string INVMT_TaxAliasName { get; set; }
        public decimal INVMIT_TaxValue { get; set; }
        public string INVTSLRET_BatchNo { get; set; }
        public string INVMS_StoreName { get; set; }
        public string returnduplicatestatus { get; set; }
        public decimal INVTSLRET_SalesReturnQty { get; set; }
        public decimal INVTSLRET_SalesReturnAmount { get; set; }
        public decimal INVMSLRET_TotalReturnAmount { get; set; }
        public string INVTSLRET_SalesReturnNaration { get; set; }
        public DateTime INVTSLRET_ReturnDate { get; set; }
        public string INVMSLRET_CreditNoteNo { get; set; }
        public string INVMSLRET_EWayRefNo { get; set; }
        public DateTime? INVMSLRET_CreditNoteDate { get; set; }
        public string INVTSLRET_ReturnNo { get; set; }
        public string INVTSLRET_ReturnNaration { get; set; }
        public string trans_id { get; set; }
        public string INVMSLRET_ReturnRemarks { get; set; }
        public bool INVTSLRET_ActiveFlg { get; set; }
        public bool returnval { get; set; }
        public DateTime INVTSLRET_CreatedDate { get; set; }
        public DateTime INVTSLRET_UpdatedDate { get; set; }
        public DateTime INVMSLRET_SalesReturnDate { get; set; }
        public long INVTSLRET_CreatedBy { get; set; }
        public long INVTSLRET_UpdatedBy { get; set; }
        public Array get_salesno { get; set; }
        public Array get_item { get; set; }
        public Array get_saleReturn { get; set; }
        public Array get_salereturnitemview { get; set; }
        public Array get_Store { get; set; }
        public Array get_itemTax { get; set; }
        public Array get_itemDetail { get; set; }
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
        public SaleItemDTO[] SaleItem { get; set; }
        public long roleId { get; set; }
        public long AMST_Id { get; set; }
        public class SaleItemDTO
        {
            public long INVTSL_Id { get; set; }
            public long INVMSL_Id { get; set; }
            public long INVMI_Id { get; set; }
            public long INVMUOM_Id { get; set; }
            public long INVMP_Id { get; set; }
            public string INVTSLRET_BatchNo { get; set; }
            public long INVSTO_Id { get; set; }
            public long INVTSLRET_SalesReturnQty { get; set; }
            public decimal INVTSLRET_SalesReturnAmount { get; set; }
            public string INVTSL_BatchNo { get; set; }
            public string INVTSLRET_SalesReturnNaration { get; set; }
            public decimal? INVTSL_SalesQty { get; set; }
            public decimal? INVTSL_SalesPrice { get; set; }
            public decimal? INVTSL_DiscountAmt { get; set; }
            public decimal? INVTSL_TaxAmt { get; set; }
            public decimal? INVTSL_Amount { get; set; }
            public string INVTSL_Naration { get; set; }
            public bool? INVTSL_ReturnFlg { get; set; }
            public decimal? INVTSL_ReturnQty { get; set; }
            public DateTime INVTSL_ReturnDate { get; set; }
            public string INVTSL_ReturnNo { get; set; }
            public string INVTSL_ReturnNaration { get; set; }
            public bool? INVTSL_ActiveFlg { get; set; }
            public Array get_itemTax { get; set; }

            public SaleItemTaxDTO[] saleItemTax { get; set; }

            public class SaleItemTaxDTO
            {
                public long INVTSLT_Id { get; set; }
                public long INVTSL_Id { get; set; }
                public long INVMT_Id { get; set; }
                public decimal INVTSLT_TaxPer { get; set; }
               
                public decimal INVTSLT_TaxAmt { get; set; }
                public bool? INVTSLT_ActiveFlg { get; set; }
            }
        }
    }
}
