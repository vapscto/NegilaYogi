using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.AssetTracking.AssetTag
{
    public class AssetTagCheckOutDTO
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
        public long INVAAT_Id { get; set; }
        public long INVMLO_Id { get; set; }
        public DateTime? INVATCO_CheckoutDate { get; set; }
        public decimal INVATCO_CheckOutQty { get; set; }
        public string INVATCO_ReceivedBy { get; set; }
        public string INVATCO_CheckOutRemarks { get; set; }
        public bool INVATCO_ActiveFlg { get; set; }
        public bool INVATCO_CheckInFlg { get; set; }
        public string INVAAT_AssetId { get; set; }
        public string INVAAT_AssetDescription { get; set; }
        public DateTime? INVAAT_ManufacturedDate { get; set; }
        public string INVAAT_SKU { get; set; }
        public string INVAAT_ModelNo { get; set; }
        public string INVAAT_SerialNo { get; set; }
        public string INVMLO_LocationRoomName { get; set; }


        public DateTime? INVAAT_PurchaseDate { get; set; }
        public Array get_store { get; set; }
        public Array get_locations { get; set; }
        public Array get_employee { get; set; }

        public Array get_items { get; set; }
        public Array get_itemtagdata { get; set; }
        public Array get_ATcheckout { get; set; }

        public tagckoutArrayDTO[] tagckoutArray { get; set; }

        //==============================Asset Tag Check Out report   
        public Array get_ckoutdetails { get; set; }
        public Array get_ckoutreport { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string optionflag { get; set; }
        public string bwdateflag { get; set; }
        public coItemArrayDTO[] coItemArray { get; set; }
        public coStoreArrayDTO[] coStoreArray { get; set; }
        public coLocationArrayDTO[] coLocationArray { get; set; }
        public coTagArrayDTO[] coTagArray { get; set; }
    }
    public class tagckoutArrayDTO
    {
        public long INVATCO_Id { get; set; }
        public long INVAAT_Id { get; set; }
        public long? HRME_Id { get; set; }
        public string INVATCO_ReceivedBy { get; set; }
        public string INVATCO_CheckOutRemarks { get; set; }
    }


    public class coItemArrayDTO
    {
        public long INVMI_Id { get; set; }
    }
    public class coStoreArrayDTO
    {
        public long INVMST_Id { get; set; }
    }
    public class coLocationArrayDTO
    {
        public long INVMLO_Id { get; set; }
    }
    public class coTagArrayDTO
    {
        public long INVAAT_Id { get; set; }
    }
}
