using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_VehicleDriver_Session", Schema = "TRN")]
    public class VehicleDriverSessionDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRVDS_Id { get; set; }
        public long TRVD_Id { get; set; }
        public long TRMS_Id { get; set; }    

    }
}
