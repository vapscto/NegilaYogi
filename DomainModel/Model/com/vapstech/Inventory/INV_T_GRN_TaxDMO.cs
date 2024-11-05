using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_T_GRN_Tax", Schema = "INV")]
    public class INV_T_GRN_TaxDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVTGRNT_Id { get; set; }
        public long INVTGRN_Id { get; set; }
        public long INVMT_Id { get; set; }
        public decimal INVTGRNT_TaxPer { get; set; }
        public decimal INVTGRNT_TaxAmt { get; set; }
        public bool INVTGRNT_ActiveFlg { get; set; }
        public INV_T_GRNDMO INV_T_GRNDMO { get; set; }


    }
}
