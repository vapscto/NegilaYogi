using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_VehicleDriver", Schema = "TRN")]
    public class VehicleDriver : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRVD_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime TRVD_Date { get; set; }
        public long TRMV_Id { get; set; }
        public long TRMD_Id { get; set; }
        public bool TRVD_ActiveFlg { get; set; }

        public List<VehicleDriverSessionDMO> VehicleDriverSessionDMO { get; set; }
    }
}
