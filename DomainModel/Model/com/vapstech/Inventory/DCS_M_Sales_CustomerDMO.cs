using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("DCS_M_Sales_Customer", Schema = "DCS")]
    public class DCS_M_Sales_CustomerDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long DCSMSLC_Id { get; set; }
        public long DCSMSL_Id { get; set; }
        public long INVMC_Id { get; set; }
        public string INVMC_GSTNO { get; set; }
        public bool? INVMSLC_ActiveFlg { get; set; }
        public DCS_M_SalesDMO DCS_M_SalesDMO { get; set; }
    }
}
