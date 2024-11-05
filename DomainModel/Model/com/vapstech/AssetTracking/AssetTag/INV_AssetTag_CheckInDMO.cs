using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.AssetTracking.AssetTag
{
    [Table("INV_AssetTag_CheckIn", Schema = "INV")]
    public class INV_AssetTag_CheckInDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long INVATCI_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMLO_Id { get; set; }
        public long INVMST_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVAAT_Id { get; set; }
        public DateTime? INVATCI_CheckInDate { get; set; }
        public decimal INVATCI_CheckInQty { get; set; }
        public string INVATCI_CheckInRemarks { get; set; }
        public long? HRME_Id { get; set; }
        public string INVATCI_ReceivedBy { get; set; }
        public bool INVATCI_ActiveFlg { get; set; }


    }
}
