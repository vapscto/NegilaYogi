using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_VehicleRoute_Session", Schema = "TRN")]
    public class VehicleRouteSessionDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRVRS_Id { get; set; }
        public long TRVR_Id { get; set; }
        public long TRMS_Id { get; set; }
    }
}
