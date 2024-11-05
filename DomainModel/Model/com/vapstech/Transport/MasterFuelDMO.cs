using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Master_FuelType", Schema = "TRN")]

    public class MasterFuelDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRMFT_Id { get; set; }
        public long MI_Id { get; set; }
        public string TRMFT_FuelType { get; set; }

      

        public bool TRMFT_ActiveFlg { get; set; }
    }
}
