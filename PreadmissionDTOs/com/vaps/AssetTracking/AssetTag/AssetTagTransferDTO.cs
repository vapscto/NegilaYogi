using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.AssetTracking.AssetTag
{
    public class AssetTagTransferDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public long MI_Id { get; set; }
        public long INVMST_Id { get; set; }
        public long INVMI_Id { get; set; }
        public string INVMS_StoreName { get; set; }
        public string INVMI_ItemName { get; set; }
        public long HRME_Id { get; set; }
        public string employeename { get; set; }
        public int? HRME_EmployeeOrder { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public long INVATCO_Id { get; set; }
        public long INVMLO_Id { get; set; }

        public long INVATTR_Id { get; set; }
        public long INVMLOFrom_Id { get; set; }
             
        public long INVAAT_Id { get; set; }
        public long INVMLOTo_Id { get; set; }
        public DateTime INVATTR_CheckoutDate { get; set; }
        public decimal INVATTR_CheckOutQty { get; set; }
        public string INVATTR_ReceivedBy { get; set; }
        public string INVATTR_CheckOutRemarks { get; set; }
        public bool INVATTR_ActiveFlg { get; set; }

        public string from_Location { get; set; }
        public string to_Location { get; set; }

        public string INVAAT_AssetId { get; set; }
        public string INVAAT_AssetDescription { get; set; }
        public DateTime? INVAAT_ManufacturedDate { get; set; }
        public string INVAAT_SKU { get; set; }
        public string INVAAT_ModelNo { get; set; }
        public string INVAAT_SerialNo { get; set; }
        public string INVMLO_LocationRoomName { get; set; }


        public DateTime? INVAAT_PurchaseDate { get; set; }
        public Array get_store { get; set; }
        public Array get_fromlocations { get; set; }
        public Array get_tolocations { get; set; }
        public Array get_employee { get; set; }

        public Array get_items { get; set; }
        public Array get_itemtagdata { get; set; }
        public Array get_ATTransfer { get; set; }

        public tagTransferArrayDTO[] tagTransferArray { get; set; }

        //==============================Asset Tag Transfer report   
        public Array get_transferdetails { get; set; }
        public Array get_transferreport { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string optionflag { get; set; }
        public string bwdateflag { get; set; }
        public disposeItemArrayDTO[] transferItemArray { get; set; }
        public disposeStoreArrayDTO[] transferStoreArray { get; set; }
        public disposeLocationArrayDTO[] transferLocationArray { get; set; }
        public disposeTagArrayDTO[] transferTagArray { get; set; }
    }
    public class tagTransferArrayDTO
    {
        public long INVATTR_Id { get; set; }
        public long INVAAT_Id { get; set; }
        public DateTime INVATTR_CheckoutDate { get; set; }
        public decimal INVATTR_CheckOutQty { get; set; }
        public string INVATTR_ReceivedBy { get; set; }
        public string INVATTR_CheckOutRemarks { get; set; }

    }
}
