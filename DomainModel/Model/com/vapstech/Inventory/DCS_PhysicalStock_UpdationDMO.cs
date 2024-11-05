using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("DCS_PhysicalStock_Updation", Schema = "DCS")]
   public class DCS_PhysicalStock_UpdationDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long DCSPSU_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMST_Id { get; set; }
        public long INVMP_Id { get; set; }
        public decimal INVPSU_StockPlus { get; set; }
        public decimal INVPSU_StockMinus { get; set; }
        public string INVPSU_Remarks { get; set; }
        public bool INVPSU_ActiveFlg { get; set; }
        public long INVPSU_CreatedBy { get; set; }
        public long INVPSU_UpdatedBy { get; set; }

        public long INVMUOM_Id { get; set; }

    }
}
