using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.AssetTracking
{
    [Table("INV_Asset_Dispose", Schema = "INV")]
    public class INV_Asset_DisposeDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVADI_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMST_Id { get; set; }
        public long INVMI_Id { get; set; }
        public decimal? INVSTO_SalesRate { get; set; }
        public long INVMLO_Id { get; set; }
        public DateTime INVADI_DisposedDate { get; set; }
        public decimal INVADI_DisposedQty { get; set; }      
        public string INVADI_DisposedRemarks { get; set; }
        public bool INVADI_ActiveFlg { get; set; }
       
    }
}
