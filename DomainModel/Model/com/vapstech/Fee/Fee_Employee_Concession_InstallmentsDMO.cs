using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Fee_Employee_Concession_Installments")]
    public class Fee_Employee_Concession_InstallmentsDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FECI_Id { get; set; }
        public long FECI_FEC_Id { get; set; }
        public long FTI_Id { get; set; }
        public long FSCI_ConcessionAmount { get; set; }
        
    }
}
