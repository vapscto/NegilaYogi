using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Hirer_Rate", Schema = "TRN")]
    public class MasterHirerRateDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRHR_Id { get; set; }
        public long MI_Id { get; set; }
        public long TRHG_Id { get; set; }
        public long TRMVT_Id { get; set; }
        public decimal? TRHR_RatePerKM { get; set; }

    }
}
