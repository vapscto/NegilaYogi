using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_M_Sales_Staff", Schema = "INV")]
    public class INV_M_Sales_StaffDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMSLST_Id { get; set; }
        public long INVMSL_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool? INVMSLST_ActiveFlg { get; set; }

        public INV_M_SalesDMO INV_M_SalesDMO { get; set; }
    }
}
