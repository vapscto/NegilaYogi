using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.AssetTracking.AssetTag
{
    [Table("INV_AssetTag_Dispose", Schema = "INV")]
    public class INV_AssetTag_DisposeDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long INVATDI_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMST_Id { get; set; }
        public long INVMLO_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVAAT_Id { get; set; }
        public DateTime? INVATDI_DisposedDate { get; set; }
        public decimal INVATDI_DisposedQty { get; set; }
        public string INVATDI_DisposedRemarks { get; set; }
        public bool INVATDI_ActiveFlg { get; set; }

    }
}
