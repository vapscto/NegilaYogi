using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("CLG_Fee_OnlinePayment_Mapping", Schema = "CLG")]
    public class CLG_Fee_OnlinePayment_MappingDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CFOPM_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMH_Id { get; set; }
        public long fpgd_id { get; set; }
        public long fmg_id { get; set; }
        public long fti_id { get; set; }
       // public long fmt_id { get; set; }
        public long AMCO_Id { get; set; }
        public bool PreAdmFlag { get; set; }
       
    }
}
