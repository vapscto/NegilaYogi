using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.AssetTracking
{
    [Table("INV_Master_Location", Schema = "INV")]
    public class INV_Master_LocationDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMLO_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMSI_Id { get; set; }
        public string INVMLO_LocationRoomName { get; set; }
        public string INVMLO_LocationRemarks { get; set; }
        public bool INVMLO_ActiveFlg { get; set; }
        public long? HRME_Id { get; set; }
        public string INVMLO_InchargeName { get; set; }

    }
}
