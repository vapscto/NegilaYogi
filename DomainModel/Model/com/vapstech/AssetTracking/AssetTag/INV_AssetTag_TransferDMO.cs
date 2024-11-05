using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.AssetTracking.AssetTag
{
    [Table("INV_AssetTag_Transfer", Schema = "INV")]
    public class INV_AssetTag_TransferDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long INVATTR_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMLOFrom_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVAAT_Id { get; set; }
        public long INVMLOTo_Id { get; set; }
        public DateTime? INVATTR_CheckoutDate { get; set; }
        public decimal INVATTR_CheckOutQty { get; set; }
        public string INVATTR_ReceivedBy { get; set; }
        public string INVATTR_CheckOutRemarks { get; set; }
        public bool INVATTR_ActiveFlg { get; set; }

    }
}
