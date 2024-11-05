using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Route_Sch_Sess_Location", Schema = "TRN")]
    public class TR_Route_Sch_Sess_LocationDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRRSCSL_Id { get; set; }
        public long TRRSCS_Id { get; set; }
        public long TRML_Id { get; set; }
        public string TRRSCSL_ArrivalTime { get; set; }
        public string TRRSCSL_DepartureTime { get; set; }
        public int TRRSCSL_Order { get; set; }
        public bool TRRSCSL_ActiveFlg { get; set; }



    }
}
