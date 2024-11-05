using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_Master_Customer", Schema = "INV")]
    public class INV_Master_CustomerDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMC_Id { get; set; }
        public long MI_Id { get; set; }
        public string INVMC_CustomerName { get; set; }
        public string INVMC_CustomerContactPerson { get; set; }
        public long INVMC_CustomerContactNo { get; set; }
        public string INVMC_CustomerAddress { get; set; }
        public bool INVMC_ActiveFlg { get; set; }





    }
}
