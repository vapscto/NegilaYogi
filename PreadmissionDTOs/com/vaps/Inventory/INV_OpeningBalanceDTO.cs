using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class INV_OpeningBalanceDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }

        public long IMFY_Id { get; set; }
        public long INVOB_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMST_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMUOM_Id { get; set; }
        public string INVOB_BatchNo { get; set; }
        public DateTime? INVOB_PurchaseDate { get; set; }
        public decimal INVOB_PurchaseRate { get; set; }
        public decimal INVOB_SaleRate { get; set; }
        public decimal INVOB_Qty { get; set; }
        public string INVOB_Naration { get; set; }
        public DateTime? INVOB_MfgDate { get; set; }
        public DateTime? INVOB_ExpDate { get; set; }
        public bool INVOB_ActiveFlg { get; set; }


        public string INVMI_ItemName { get; set; }
        public string INVMS_StoreName { get; set; }
        public string INVMUOM_UOMName { get; set; }
        public string INVMUOM_UOMAliasName { get; set; }
        public string INVC_LIFOFIFOFlg { get; set; }
        public string EntryType { get; set; }
        public Array year_list_ob { get; set; }
        public Array store_list_ob { get; set; }
        public Array ob_report_list { get; set; }


        public Array get_Store { get; set; }
        public Array year_id { get; set; }
        public Array get_item { get; set; }
        public Array get_obdetails { get; set; }
        public Array get_itemDetail { get; set; }
        public Array get_openingbalance { get; set; }
        public Array get_store { get; set; }

        public OBDTO[] OBItem { get; set; }

    }

    public class OBDTO
    {
        public long INVMI_Id { get; set; }
        public long INVMUOM_Id { get; set; }
        public string INVOB_BatchNo { get; set; }
        public DateTime? INVOB_PurchaseDate { get; set; }
        public decimal INVOB_PurchaseRate { get; set; }
        public decimal INVOB_SaleRate { get; set; }

        public decimal INVOB_Amount { get; set; }
        public decimal INVOB_Qty { get; set; }
        public string INVOB_Naration { get; set; }
        public DateTime? INVOB_MfgDate { get; set; }
        public DateTime? INVOB_ExpDate { get; set; }

    }
}
