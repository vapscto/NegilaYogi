using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Y_Payment_Preadmission_Registration")]
    public class Fee_Y_Payment_Preadmission_RegistrationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FYPPR_Id { get; set; }
        public long FYP_Id { get; set; }
        public long PASR_Id { get; set; }
        public long FYPPR_TotalPaidAmount { get; set; }
        public long FYPPR_ActiveFlag { get; set; }
    }
}
