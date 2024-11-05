using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Hirer_OB", Schema = "TRN")]
    public class TR_Hirer_OB_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRHOB_Id { get; set; }
        public long MI_Id { get; set; }
        public long TRMH_Id { get; set; }
        public decimal TRHOB_DueAmount { get; set; }
        public decimal TRHOB_ExcessAmount { get; set; }
        public bool TRHOB_ClearedFlag{get;set;}
        public bool TRHOB_ExcessClearedFlag { get; set; }
    }
}
