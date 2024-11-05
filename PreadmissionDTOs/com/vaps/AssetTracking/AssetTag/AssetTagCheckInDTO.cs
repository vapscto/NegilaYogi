using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.AssetTracking.AssetTag
{
    public class AssetTagCheckInDTO
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
        public long? HRME_Id { get; set; }
        public string employeename { get; set; }
        public int? HRME_EmployeeOrder { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public long INVATCO_Id { get; set; }

        public long INVATCI_Id { get; set; }
        public long INVMLO_Id { get; set; }
        public long INVAAT_Id { get; set; }
        public DateTime? INVATCI_CheckInDate { get; set; }
        public decimal INVATCI_CheckInQty { get; set; }
        public string INVATCI_CheckInRemarks { get; set; }
        public string INVATCI_ReceivedBy { get; set; }
        public bool INVATCI_ActiveFlg { get; set; }

        public string INVAAT_AssetId { get; set; }
        public string INVAAT_AssetDescription { get; set; }
        public DateTime? INVAAT_ManufacturedDate { get; set; }
        public string INVAAT_SKU { get; set; }
        public string INVAAT_ModelNo { get; set; }
        public string INVAAT_SerialNo { get; set; }
        public string INVMLO_LocationRoomName { get; set; }
        public string contactflag { get; set; }

        public Array get_contactperson { get; set; }
        public DateTime? INVAAT_PurchaseDate { get; set; }
        public Array get_store { get; set; }
        public Array get_locations { get; set; }
        public Array get_employee { get; set; }

        public Array get_items { get; set; }
        public Array get_itemtagdata { get; set; }
        public Array get_ATcheckin { get; set; }

        public tagckInArrayDTO[] tagckInArray { get; set; }
        //==============================Asset Tag Check Out report   
        public Array get_ckIndetails { get; set; }
        public Array get_ckInreport { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string optionflag { get; set; }
        public string bwdateflag { get; set; }
        public coItemArrayDTO[] ciItemArray { get; set; }
        public coStoreArrayDTO[] ciStoreArray { get; set; }
        public coLocationArrayDTO[] ciLocationArray { get; set; }
        public ciTagArrayDTO[] ciTagArray { get; set; }
    }
    public class tagckInArrayDTO
    {
        public long INVATCI_Id { get; set; }
        public long INVAAT_Id { get; set; }
        public decimal INVATCI_CheckInQty { get; set; }
        public string INVATCI_CheckInRemarks { get; set; }
        public long? HRME_Id { get; set; }
        public string INVATCI_ReceivedBy { get; set; }

    }


    public class ciItemArrayDTO
    {
        public long INVMI_Id { get; set; }
    }
    public class ciStoreArrayDTO
    {
        public long INVMST_Id { get; set; }
    }
    public class ciLocationArrayDTO
    {
        public long INVMLO_Id { get; set; }
    }
    public class ciTagArrayDTO
    {
        public long INVAAT_Id { get; set; }
    }

}
