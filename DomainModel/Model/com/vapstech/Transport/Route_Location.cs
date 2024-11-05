using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Route_Location", Schema = "TRN")]
    public class Route_Location : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRMRL_Id { get; set; }
        public long MI_Id { get; set; }
        public long TRMR_Id { get; set; }
        public long TRML_Id { get; set; }
        public int TRMRL_Order { get; set; }
        public bool TRMRL_ActiveFlag { get; set; }

    }
}
