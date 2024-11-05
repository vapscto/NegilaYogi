using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class INV_PhyStock_UpdationDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public long ASMAY_Id { get; set; }
        public long UserId { get; set; }

        public long IMFY_Id { get; set; }
        public long INVOB_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMST_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMUOM_Id { get; set; }
        public long INVPSU_Id { get; set; }
        public decimal INVPSU_StockPlus { get; set; }
        public decimal INVPSU_StockMinus { get; set; }
        public string INVPSU_Remarks { get; set; }
        public bool INVPSU_ActiveFlg { get; set; }
        public long INVPSU_CreatedBy { get; set; }
        public long INVPSU_UpdatedBy { get; set; }

        public string INVMI_ItemName { get; set; }
        public string INVMS_StoreName { get; set; }
        public string INVMUOM_UOMName { get; set; }
        public string INVMUOM_UOMAliasName { get; set; }
        public string INVC_LIFOFIFOFlg { get; set; }
        public decimal? INVSTO_AvaiableStock { get; set; }
        public decimal? INVSTO_SalesRate { get; set; }
        public Array get_Store { get; set; }
        public Array get_item { get; set; }
        public Array get_phyStockdata { get; set; }
        public phyStockDTO[] phyStock { get; set; }
        public phyStockDTO1[] DCSphyStock { get; set; }

        // product

        public long DCSPSU_Id { get; set; }
        public string INVMP_ProductName { get; set; }
        public long INVMP_Id { get; set; }
        public Array get_itemDetail { get; set; }
        public Array availablestock { get; set; }
        public Array get_itemTax { get; set; }
        public Array UOM { get; set; }
        public decimal? INVMP_ProductPrice { get; set; }

        // product
    }

    public class phyStockDTO1
    {
        public long DCSPSU_Id { get; set; }
        public long INVMP_Id { get; set; }
        public decimal INVPSU_StockPlus { get; set; }
        public decimal INVPSU_StockMinus { get; set; }
        public string INVPSU_Remarks { get; set; }
        public bool INVPSU_ActiveFlg { get; set; }
        public decimal INVMP_ProductPrice { get; set; }

    }

    public class phyStockDTO
    {
        public long INVPSU_Id { get; set; }
        public long INVMI_Id { get; set; }
        public decimal INVPSU_StockPlus { get; set; }
        public decimal INVPSU_StockMinus { get; set; }
        public string INVPSU_Remarks { get; set; }
        public bool INVPSU_ActiveFlg { get; set; }
        public decimal INVSTO_SalesRate { get; set; }

    }
}
