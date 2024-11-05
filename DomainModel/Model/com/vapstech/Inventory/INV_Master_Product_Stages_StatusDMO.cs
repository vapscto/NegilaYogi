using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_Master_Product_Stages_Status", Schema = "DCS")]
   public class INV_Master_Product_Stages_StatusDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long INVMPSS_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMPS_Id { get; set; }
        public decimal INVMPSS_Status { get; set; }
        public bool INVMPSS_ActiveFlg { get; set; }
        public long INVMP_Id { get; set; }


    }
}
