using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Vehicle_Driver_Allottment",Schema ="TRN")]
    public class TRVehicleDriverAllottmentDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRVDA_Id { get; set; }
        public long TRTP_Id { get; set; }
        public long TRMV_Id { get; set; }
        public long TRVD_Id { get; set; }
        public long? TRTP_OpeningKM { get; set; }
        public long? TRTP_ClosingKM { get; set; }
    }
}
