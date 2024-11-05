using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.AssetTracking
{
    public class TransferAssetsDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public long INVATR_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMLOFrom_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMLOTo_Id { get; set; }
        public long? HRME_Id { get; set; }
        public DateTime INVATR_CheckoutDate { get; set; }
        public decimal INVATR_CheckOutQty { get; set; }
        public string INVATR_ReceivedBy { get; set; }
        public string INVATR_CheckOutRemarks { get; set; }
        public bool INVATR_ActiveFlg { get; set; }
        public long INVMLO_Id { get; set; }
        public string INVMLO_LocationRoomName { get; set; }
        public string INVMLO_LocationRemarks { get; set; }
        public string INVMI_ItemName { get; set; }

        public string FINVMLO_LocationRoomName { get; set; }
        public string TINVMLO_LocationRoomName { get; set; }
        public decimal? INVSTO_SalesRate { get; set; }
        public string INVC_LIFOFIFOFlg { get; set; }
        public Array get_transfer { get; set; }
        public Array get_itemdetails { get; set; }
        public Array get_locations { get; set; }
        public Array get_employee { get; set; }
        public Array get_items { get; set; }
        public Array get_tolocations { get; set; }



    }
}
