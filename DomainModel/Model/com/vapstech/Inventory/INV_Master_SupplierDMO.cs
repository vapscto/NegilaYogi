using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_Master_Supplier", Schema = "INV")]
    public class INV_Master_SupplierDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMS_Id { get; set; }
        public long MI_Id { get; set; }
        public string INVMS_SupplierName { get; set; }
        public string INVMS_SupplierCode { get; set; }
        public string INVMS_SupplierConatctPerson { get; set; }
        public long INVMS_SupplierConatctNo { get; set; }
        public string INVMS_EmailId { get; set; }
        public string INVMS_GSTNo { get; set; }
        public string INVMS_SupplierAddress { get; set; }
        public bool INVMS_ActiveFlg { get; set; }






    }
}
