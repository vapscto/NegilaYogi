using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_Configuration", Schema = "INV")]
    public class INV_ConfigurationDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long INVC_Id { get; set; }
        public long MI_Id { get; set; }
        public string INVC_LIFOFIFOFlg { get; set; }
        public bool INVC_ProcessApplFlg { get; set; }
        public bool INVC_PRApplicableFlg { get; set; }
        public long INVMST_Id { get; set; }
        public int INVC_AlertsBeforeDays { get; set; }
       
     

    }
}
