using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_C_Student_Concession_Installments", Schema = "CLG")]
    public class CollegeConcessionInstallmentDMO  :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FSCI_Id { get; set; }
        public long FCSC_Id { get; set; }
        public long FTI_Id { get; set; }

        public long FSCI_ConcessionAmount { get; set; }
        public long? FSCI_UpdatedBy { get; set; }

        public long? FSCI_CreatedBy { get; set; }




    }
}
