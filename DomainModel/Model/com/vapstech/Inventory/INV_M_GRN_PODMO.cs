using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_M_GRN_PO", Schema = "INV")]
    public class INV_M_GRN_PODMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMGRNPO_Id { get; set; }
        public long INVMGRN_Id { get; set; }
        public long INVMPO_Id { get; set; }

    }
}
