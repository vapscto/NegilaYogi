using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_M_GRN_Store", Schema = "INV")]
    public class INV_M_GRN_StoreDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMGRNS_Id { get; set; }
        public long INVMGRN_Id { get; set; }
        public long INVMST_Id { get; set; }
        public INV_M_GRNDMO INV_M_GRNDMO { get; set; }
    }
}
