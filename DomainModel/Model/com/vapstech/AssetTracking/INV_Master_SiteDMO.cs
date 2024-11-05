using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.AssetTracking
{
    [Table("INV_Master_Site", Schema = "INV")]
    public class INV_Master_SiteDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMSI_Id { get; set; }
        public long MI_Id { get; set; }
        public string INVMSI_SiteBuildingName { get; set; }
        public string INVMSI_SiteRemarks { get; set; }
        public bool INVMSI_ActiveFlg { get; set; }


    }
}
