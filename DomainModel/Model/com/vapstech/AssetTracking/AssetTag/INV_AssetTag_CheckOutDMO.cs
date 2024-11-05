using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.AssetTracking.AssetTag
{
    [Table("INV_AssetTag_CheckOut", Schema = "INV")]
    public class INV_AssetTag_CheckOutDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long INVATCO_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMST_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVAAT_Id { get; set; }
        public long INVMLO_Id { get; set; }
        public DateTime? INVATCO_CheckoutDate { get; set; }
        public decimal INVATCO_CheckOutQty { get; set; }
        public long? HRME_Id { get; set; }
        public string INVATCO_ReceivedBy { get; set; }
        public string INVATCO_CheckOutRemarks { get; set; }
        public bool INVATCO_CheckInFlg { get; set; }
        public bool INVATCO_ActiveFlg { get; set; }


    }
}
