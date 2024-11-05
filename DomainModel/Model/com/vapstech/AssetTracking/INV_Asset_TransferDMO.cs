using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.AssetTracking
{
    [Table("INV_Asset_Transfer", Schema = "INV")]
    public class INV_Asset_TransferDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVATR_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMLOFrom_Id { get; set; }
        public long INVMI_Id { get; set; }
        public decimal? INVSTO_SalesRate { get; set; }
        public long INVMLOTo_Id { get; set; }
        public long? HRME_Id { get; set; }
        public DateTime INVATR_CheckoutDate { get; set; }
        public decimal INVATR_CheckOutQty { get; set; }
        public string INVATR_ReceivedBy { get; set; }
        public string INVATR_CheckOutRemarks { get; set; }
        public bool INVATR_ActiveFlg { get; set; }
   


    }
}
