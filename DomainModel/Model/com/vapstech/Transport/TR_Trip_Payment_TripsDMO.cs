using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Trip_Payment_Trips", Schema = "TRN")]
    public class TR_Trip_Payment_TripsDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long  TRTPPT_Id { get; set; }
        public long TRTPP_Id { get; set; }

        public long TRTP_Id { get; set; }
        public bool TRTPPT_ActiveFlag { get; set; }
    }
}
