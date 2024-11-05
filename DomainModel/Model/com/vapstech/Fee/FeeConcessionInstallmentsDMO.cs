using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Student_Concession_Installments")]
    public class FeeConcessionInstallmentsDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FSCI_ID { get; set; }
        public long FSCI_FSC_Id { get; set; }
        public long FTI_Id { get; set; }
        public long FSCI_ConcessionAmount { get; set; }

        public long? FSCI_CreatedBy { get; set; }
        public long? FSCI_UpdatedBy { get; set; }
    }
}
