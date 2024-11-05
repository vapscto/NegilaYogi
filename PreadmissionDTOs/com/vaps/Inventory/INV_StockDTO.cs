using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class INV_StockDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }

        public long INVSTO_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMST_Id { get; set; }
        public long INVMG_Id { get; set; }
        public long INVMI_Id { get; set; }
        public DateTime? INVSTO_PurchaseDate { get; set; }
        public decimal? INVSTO_PurchaseRate { get; set; }
        public string INVSTO_BatchNo { get; set; }
        public decimal? INVSTO_SalesRate { get; set; }
        public decimal? INVSTO_PurOBQty { get; set; }
        public decimal? INVSTO_PurRetQty { get; set; }
        public decimal? INVSTO_SalesQty { get; set; }
        public decimal? INVSTO_SalesRetQty { get; set; }
        public decimal? INVSTO_ItemConQty { get; set; }
        public decimal? INVSTO_MatIssPlusQty { get; set; }
        public decimal? INVSTO_MatIssMinusQty { get; set; }
        public decimal? INVSTO_PhyPlusQty { get; set; }
        public decimal? INVSTO_PhyMinQty { get; set; }
        public decimal? INVSTO_AvaiableStock { get; set; }

        public string INVMS_StoreName { get; set; }
        public string INVMI_ItemName { get; set; }
        //==========================Reports
        public Array get_items { get; set; }
        public Array get_StockReport { get; set; }
        public stockItemArrayDTO[] stockItemArray { get; set; }
        public stockStoreArrayDTO[] stockStoreArray { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string optionflag { get; set; }
        public string overallflag { get; set; }
        //========================================
        public Array get_stockdetails { get; set; }
        public Array get_item { get; set; }
        public Array get_store { get; set; }
        public Array get_stock { get; set; }
        public Array get_editstock { get; set; }

        public StockItemDTO[] stocklist { get; set; }
        public Array get_product { get; set; }
        public long INVMP_Id { get; set; }
    }

    public class StockItemDTO
    {
        public long INVSTO_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMST_Id { get; set; }
        public long INVMI_Id { get; set; }
        public DateTime? INVSTO_PurchaseDate { get; set; }
        public decimal? INVSTO_PurchaseRate { get; set; }
        public string INVSTO_BatchNo { get; set; }
        public decimal? INVSTO_SalesRate { get; set; }
        public decimal? INVSTO_PurOBQty { get; set; }
        public decimal? INVSTO_PurRetQty { get; set; }
        public decimal? INVSTO_SalesQty { get; set; }
        public decimal? INVSTO_SalesRetQty { get; set; }
        public decimal? INVSTO_ItemConQty { get; set; }
        public decimal? INVSTO_MatIssPlusQty { get; set; }
        public decimal? INVSTO_MatIssMinusQty { get; set; }
        public decimal? INVSTO_PhyPlusQty { get; set; }
        public decimal? INVSTO_PhyMinQty { get; set; }
        public decimal? INVSTO_AvaiableStock { get; set; }
    }

    public class stockItemArrayDTO
    {
        public long INVMI_Id { get; set; }
        public string INVMI_ItemName { get; set; }

    }
    public class stockStoreArrayDTO
    {
        public long INVMST_Id { get; set; }      

    }
}
