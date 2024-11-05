using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_Master_Tax", Schema = "INV")]
    public class INV_Master_TaxDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMT_Id { get; set; }
        public long MI_Id { get; set; }
        public string INVMT_TaxName { get; set; }
        public string INVMT_TaxAliasName { get; set; }
        public bool INVMT_ActiveFlg { get; set; }



    }
}
