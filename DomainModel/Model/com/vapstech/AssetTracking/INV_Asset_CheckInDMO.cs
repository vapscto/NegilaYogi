using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.AssetTracking
{
    [Table("INV_Asset_CheckIn", Schema = "INV")]
    public class INV_Asset_CheckInDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVACI_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMST_Id { get; set; }
        public long INVMI_Id { get; set; }
        public decimal? INVSTO_SalesRate { get; set; }
        public long INVMLO_Id { get; set; }
        public DateTime INVACI_CheckInDate { get; set; }
        public decimal INVACI_CheckInQty { get; set; }
        public string INVACI_ReceivedBy { get; set; }
        public string INVACI_CheckInRemarks { get; set; }
        public bool INVACI_ActiveFlg { get; set; }
        public long? HRME_Id { get; set; }

    }
}
