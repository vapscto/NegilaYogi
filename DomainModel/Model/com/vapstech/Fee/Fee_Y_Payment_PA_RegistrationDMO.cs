using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Fee_Y_Payment_PA_Registration")]
    public class Fee_Y_Payment_PA_RegistrationDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FYPPR_Id { get; set; }
        public long FYP_Id { get; set; }
        public long PASR_Id { get; set; }
        public long FYPPR_TotalPaidAmount { get; set; }
        public bool FYPPR_ActiveFlag { get; set; }

    }
}
