using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.AssetTracking.AssetTag
{
    public class AssetTagDisposeDTO
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
        public long INVATDI_Id { get; set; }     
        public long INVMLO_Id { get; set; }    
        public long INVAAT_Id { get; set; }
        public DateTime INVATDI_DisposedDate { get; set; }
        public decimal INVATDI_DisposedQty { get; set; }
        public string INVATDI_DisposedRemarks { get; set; }
        public bool INVATDI_ActiveFlg { get; set; }

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
        public Array get_ATcheckin { get; set; }

        public tagDisposeArrayDTO[] tagDisposeArray { get; set; }
        //==============================Asset Tag Dispose report   
        public Array get_disposedetails { get; set; }
        public Array get_disposereport { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string optionflag { get; set; }
        public string bwdateflag { get; set; }
        public disposeItemArrayDTO[] disposeItemArray { get; set; }
        public disposeStoreArrayDTO[] disposeStoreArray { get; set; }
        public disposeLocationArrayDTO[] disposeLocationArray { get; set; }
        public disposeTagArrayDTO[] disposeTagArray { get; set; }
    }
    public class tagDisposeArrayDTO
    {
        public long INVATDI_Id { get; set; }
        public long INVMLO_Id { get; set; }
        public long INVAAT_Id { get; set; }
        public DateTime INVATDI_DisposedDate { get; set; }
        public decimal INVATDI_DisposedQty { get; set; }
        public string INVATDI_DisposedRemarks { get; set; }
    }


    public class disposeItemArrayDTO
    {
        public long INVMI_Id { get; set; }
    }
    public class disposeStoreArrayDTO
    {
        public long INVMST_Id { get; set; }
    }
    public class disposeLocationArrayDTO
    {
        public long INVMLO_Id { get; set; }
    }
    public class disposeTagArrayDTO
    {
        public long INVAAT_Id { get; set; }
    }

}
