using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Purchase.Inventory
{
    public class INV_PurchaseOrderDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long UserId { get; set; }

        public string trans_id { get; set; }
        public long INVMI_Id { get; set; }
        public string INVMI_ItemName { get; set; }
        public long INVMIT_Id { get; set; }
        public long INVMT_Id { get; set; }
        public string INVMT_TaxName { get; set; }
        public string INVMT_TaxAliasName { get; set; }
        public decimal INVMIT_TaxValue { get; set; }
        public string INVMI_ItemCode { get; set; }
        public long INVMUOM_Id { get; set; }
        public string INVMUOM_UOMName { get; set; }
        public string INVMUOM_UOMAliasName { get; set; }
        public long INVMPO_Id { get; set; }
        public long INVMSQ_Id { get; set; }
        public long INVMPI_Id { get; set; }
        public long INVTPI_Id { get; set; }
        public long INVTSQ_Id { get; set; }
        public long? INVMS_Id { get; set; }
        public string INVMPO_PONo { get; set; }
        public DateTime INVMPO_PODate { get; set; }
        public string INVMPO_Remarks { get; set; }
        public string INVMPO_ReferenceNo { get; set; }
        public decimal INVMSQ_NegotiatedRate { get; set; }
        public decimal INVMPO_TotRate { get; set; }
        public decimal INVMPO_TotTax { get; set; }
        public decimal INVMPO_TotAmount { get; set; }
        public bool INVMPO_ActiveFlg { get; set; }
        public long INVMPO_CreatedBy { get; set; }
        public long INVMPO_UpdatedBy { get; set; }
        public decimal INVTPI_PIQty { get; set; }
        public decimal INVTPI_PIUnitRate { get; set; }
        public decimal INVTPI_ApproxAmount { get; set; }
        public long INVTPO_Id { get; set; }
        public decimal INVTPO_POQty { get; set; }
        public decimal INVTPO_RatePerUnit { get; set; }
        public decimal INVTPO_TaxAmount { get; set; }
        public decimal INVTPO_Amount { get; set; }
        public string INVTPO_Remarks { get; set; }
        public bool INVTPO_ActiveFlg { get; set; }
        public long INVTPO_CreatedBy { get; set; }
        public long INVTPO_UpdatedBy { get; set; }

        public long INVTPOT_Id { get; set; }
        public decimal INVTPOT_TaxPercent { get; set; }
        public decimal INVTPOT_TaxAmount { get; set; }
        public bool INVTPOT_ActiveFlg { get; set; }
        public long INVTPOT_CreatedBy { get; set; }
        public long INVTPOT_UpdatedBy { get; set; }


        public decimal INVTSQ_QuotedRate { get; set; }
        public decimal INVTSQ_NegotiatedRate { get; set; }

        public string quotationflag { get; set; }
        public string Selectionflag { get; set; }
        

        public Array get_quotationno { get; set; }
        public Array get_comparequotationno { get; set; }
        public Array get_purchaseorder { get; set; }
        public Array get_supplier { get; set; }
        public Array get_PI { get; set; }
        
        public Array get_qtdetails { get; set; }
        public Array pidetails { get; set; }
        
        public Array get_pisupplier { get; set; }
        public Array get_itemTax { get; set; }
        public Array get_poDetail { get; set; }
        public Array get_supdata { get; set; }
        
        public Array get_potax { get; set; }
        public Array get_editDetail { get; set; }

        public List<INV_PurchaseOrderDTO> itemtax { get; set; }
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
        public arrayPODTO[] arrayPO { get; set; }
        //===================== PO Report
        public Array get_POdetails { get; set; }
        public Array get_POreport { get; set; }      
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string optionflag { get; set; }
        public string bwdateflag { get; set; }
        //================report PO    
        public poArrayDTO[] poArray { get; set; }
        public poitmArrayDTO[] poitemArray { get; set; }
        public posuplierArrayDTO[] posuplierArray { get; set; }

    }

    //=======================PO Trans
    public class arrayPODTO
    {
        public long INVTPO_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMUOM_Id { get; set; }
        public long INVMPI_Id { get; set; }
        public decimal INVTPO_POQty { get; set; }
        public decimal INVTPO_RatePerUnit { get; set; }
        public decimal INVTPO_TaxAmount { get; set; }
        public decimal INVTPO_Amount { get; set; }
        public string INVTPO_Remarks { get; set; }
        public bool INVTPO_ActiveFlg { get; set; }
        public long INVTPO_CreatedBy { get; set; }
        public long INVTPO_UpdatedBy { get; set; }
        public DateTime? INVTPO_ExpectedDeliveryDate { get; set; }


        public arrayPOtaxDTO[] arrayPOtax { get; set; }

    }
    //======================= PO TAX
    public class arrayPOtaxDTO
    {
        public long INVTPOT_Id { get; set; }
        public long INVMIT_Id { get; set; }
        public decimal INVTPOT_TaxPercent { get; set; }
        public decimal INVTPOT_TaxAmount { get; set; }
        public bool INVTPOT_ActiveFlg { get; set; }
        public long INVTPOT_CreatedBy { get; set; }
        public long INVTPOT_UpdatedBy { get; set; }

    }

    public class poArrayDTO
    {
        public long INVMPO_Id { get; set; }
    }
    public class poitmArrayDTO
    {
        public long INVMI_Id { get; set; }
    }
    public class posuplierArrayDTO
    {
        public long INVMS_Id { get; set; }
    }

}
