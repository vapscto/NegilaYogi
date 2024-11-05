using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Purchase.Inventory
{
    public class INV_QuotationDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public long UserId { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string trans_id { get; set; }
        public long INVMI_Id { get; set; }
        public string INVMI_ItemName { get; set; }
        public string INVMI_ItemCode { get; set; }
        public long INVMUOM_Id { get; set; }
        public string INVMUOM_UOMName { get; set; }
        public long INVMPI_Id { get; set; }
        public string INVMPI_PINo { get; set; }
        public DateTime INVMPI_PIDate { get; set; }
        public string INVMPI_Remarks { get; set; }
        public long INVMSQ_Id { get; set; }
        public string INVMSQ_QuotationNo { get; set; }
        public string INVMSQ_SupplierName { get; set; }
        public long INVMSQ_SupplierContactNo { get; set; }
        public string INVMSQ_SupplierEmailId { get; set; }
        public string INVMSQ_Quotation { get; set; }
        public decimal INVMSQ_TotalQuotedRate { get; set; }
        public decimal INVMSQ_NegotiatedRate { get; set; }
        public string INVMSQ_Remarks { get; set; }
        public bool INVMSQ_FinaliseFlg { get; set; }
        public bool INVMSQ_ActiveFlg { get; set; }
        public long INVMSQ_CreatedBy { get; set; }
        public long INVMSQ_UpdatedBy { get; set; }

        public long INVTSQ_Id { get; set; }
        public decimal INVTSQ_QuotedRate { get; set; }
        public decimal INVTSQ_NegotiatedRate { get; set; }
        public bool INVTSQ_FinaliseFlg { get; set; }
        public bool INVTSQ_ActiveFlg { get; set; }
        public long INVTSQ_CreatedBy { get; set; }
        public long INVTSQ_UpdatedBy { get; set; }


        public Array get_pidetails { get; set; }
        public Array get_piNo { get; set; }
        public Array get_QuotationNo { get; set; }
        public Array get_quotationdetails { get; set; }

        public Array get_supplier { get; set; }
        public Array get_Qtsupplier { get; set; }
        public Array get_pisupplier { get; set; }
        public Array get_Quotation { get; set; }
        public Array get_qtdetails { get; set; }
        public Array get_Comparison { get; set; }

        //===================== PR Report
        public Array get_Quotedetails { get; set; }
        public Array get_Quotationreport { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string optionflag { get; set; }
        public string bwdateflag { get; set; }

        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }

        public arrayQuatationDTO[] arrayQuatation { get; set; }
        public arrayQSupplierDTO[] arrayQSupplier { get; set; }
        public arrayQcompareDTO[] arrayQcompare { get; set; }
        //================report Quotation
        public quoteArrayDTO[] quoteArray { get; set; }
        public pIArrayDTO[] pIArray { get; set; }
        public itmArrayDTO[] itemArray { get; set; }
        public suplierArrayDTO[] suplierArray { get; set; }
    }
    public class arrayQuatationDTO
    {
        public long INVMSQ_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMUOM_Id { get; set; }
        public long INVTSQ_Id { get; set; }
        public decimal INVTSQ_QuotedRate { get; set; }
        public decimal INVTSQ_NegotiatedRate { get; set; }

    }
    public class arrayQSupplierDTO
    {
        public long INVMSQ_Id { get; set; }
    }
    public class arrayQcompareDTO
    {
        public long INVMSQ_Id { get; set; }
        public string INVMSQ_SupplierName { get; set; }
        public long INVMSQ_SupplierContactNo { get; set; }
        public string INVMSQ_SupplierEmailId { get; set; }
        public string INVMSQ_Quotation { get; set; }
        public decimal INVMSQ_TotalQuotedRate { get; set; }
        public decimal INVMSQ_NegotiatedRate { get; set; }
        public string INVMSQ_Remarks { get; set; }
        public bool INVMSQ_FinaliseFlg { get; set; }
        public long INVMI_Id { get; set; }
        public string INVMI_ItemName { get; set; }
        public long INVMUOM_Id { get; set; }
        public string INVMUOM_UOMName { get; set; }
        public long INVTSQ_Id { get; set; }
        public decimal INVTSQ_QuotedRate { get; set; }
        public decimal INVTSQ_NegotiatedRate { get; set; }

    }
    public class quoteArrayDTO
    {
        public long INVMSQ_Id { get; set; }
    }
    public class pIArrayDTO
    {
        public long INVMPI_Id { get; set; }
    }
    public class itmArrayDTO
    {
        public long INVMI_Id { get; set; }
    }
    public class suplierArrayDTO
    {
        public long INVMSQ_Id { get; set; }
    }


}
