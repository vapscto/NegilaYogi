using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_VehicleRoute", Schema = "TRN")]
    public class VehicleRouteDMo : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRVR_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime TRVR_Date { get; set; }
        public long TRMV_Id { get; set; }
        public long TRMR_Id { get; set; }
        public bool TRVR_ActiveFlg { get; set; }
        public List<VehicleRouteSessionDMO> VehicleRouteSessionDMO { get; set; }

    }
}
