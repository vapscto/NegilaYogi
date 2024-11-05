using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Master_VehicleType", Schema = "TRN")]

 
    public class MasterVehicleTypeDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRMVT_Id { get; set; }
        public long MI_Id { get; set; }
        public string TRMVT_VehicleType { get; set; }

        public string TRMVT_VehicleDesc { get; set; }

        public bool TRMVT_ActiveFlg { get; set; }
    
    }
}
