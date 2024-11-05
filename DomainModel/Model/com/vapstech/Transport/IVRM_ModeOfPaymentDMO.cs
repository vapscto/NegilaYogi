using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("IVRM_ModeOfPayment")]
    public class IVRM_ModeOfPaymentDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMMOD_Id { get; set; }
        public long MI_Id { get; set; }
        public string IVRMMOD_ModeOfPayment { get; set; }
        public string IVRMMOD_Flag { get; set; }
        public bool IVRMMOD_ActiveFlag { get; set; }
    }
}
