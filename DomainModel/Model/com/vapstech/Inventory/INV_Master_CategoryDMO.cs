using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_Master_Category", Schema = "INV")]
    public class INV_Master_CategoryDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMC_Id { get; set; }
        public long MI_Id { get; set; }
        public string INVMC_CategoryName { get; set; }
        public string INVMC_AliasName { get; set; }
        public string INVMC_ParentId { get; set; }
        public long INVMC_Level { get; set; }
        public bool INVMC_ActiveFlg { get; set; }
        public long INVMC_CreatedBy { get; set; }
        public long INVMC_UpdatedBy { get; set; }
    }
}
