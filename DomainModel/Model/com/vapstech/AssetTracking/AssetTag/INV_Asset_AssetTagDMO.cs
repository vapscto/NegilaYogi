using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.AssetTracking.AssetTag
{
    [Table("INV_Asset_AssetTag", Schema = "INV")]
    public class INV_Asset_AssetTagDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVAAT_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMST_Id { get; set; }
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
        public bool INVAAT_CheckOutFlg { get; set; }
        public bool INVAAT_DisposedFlg { get; set; }
        public bool INVAAT_ActiveFlg { get; set; }


    }
}
