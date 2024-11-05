using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.AssetTracking.AssetTag
{
    public class AssetTagDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public long MI_Id { get; set; }
        public long INVMST_Id { get; set; }
        public string INVMS_StoreName { get; set; }
        public string INVMI_ItemName { get; set; }
        public long INVAAT_Id { get; set; }
        public long INVMI_Id { get; set; }
        public string INVAAT_AssetId { get; set; }
        public string INVAAT_AssetDescription { get; set; }
        public string INVAAT_ManufacturerName { get; set; }
        public DateTime? INVAAT_ManufacturedDate { get; set; }
        public string INVAAT_SKU { get; set; }
        public string INVAAT_ModelNo { get; set; }
        public string INVAAT_SerialNo { get; set; }
        public DateTime? INVAAT_PurchaseDate { get; set; }
        public string INVAAT_WarantyPeriod { get; set; }
        public DateTime? INVAAT_WarantyExpiryDate { get; set; }
        public bool INVAAT_UnderAMCFlg { get; set; }
        public DateTime? INVAAT_AMCExpiryDate { get; set; }
        public bool INVAAT_ActiveFlg { get; set; }
        public bool INVAAT_CheckOutFlg { get; set; }
        public bool INVAAT_DisposedFlg { get; set; }

        public Array get_store { get; set; }
        public Array get_Assetstag { get; set; }
        public Array get_tagdata { get; set; }
        public tagckdArrayDTO[] tagckdArray { get; set; }

        //==============================Asset Tag report   
        public Array get_tagdetails { get; set; }
        public Array get_tagreport { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string optionflag { get; set; }
        public string bwdateflag { get; set; }

        public tagItemArrayDTO[] tagItemArray { get; set; }
        public tagStoreArrayDTO[] tagStoreArray { get; set; }
    }
    public class tagckdArrayDTO
    {
        public long INVAAT_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMST_Id { get; set; }
        public long INVMI_Id { get; set; }
        public string INVAAT_AssetId { get; set; }
        public string INVAAT_AssetDescription { get; set; }
        public DateTime? INVAAT_ManufacturedDate { get; set; }
        public string INVAAT_ManufacturerName { get; set; }
        public string INVAAT_SKU { get; set; }
        public string INVAAT_ModelNo { get; set; }
        public string INVAAT_SerialNo { get; set; }
        public DateTime? INVAAT_PurchaseDate { get; set; }
        public string INVAAT_WarantyPeriod { get; set; }
        public DateTime? INVAAT_WarantyExpiryDate { get; set; }
        public bool INVAAT_UnderAMCFlg { get; set; }
        public DateTime? INVAAT_AMCExpiryDate { get; set; }
        public bool INVAAT_ActiveFlg { get; set; }
    }

    public class tagItemArrayDTO
    {
        public long INVMI_Id { get; set; }
    }
    public class tagStoreArrayDTO
    {
        public long INVMST_Id { get; set; }
    }
}
