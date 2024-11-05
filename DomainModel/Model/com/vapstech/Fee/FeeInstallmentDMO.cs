using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Master_Installment")]
    public class FeeInstallmentDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMI_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMI_Name { get; set; }
        public long FMI_No_Of_Installments { get; set; }
        public string FMI_Installment_Type { get; set; }
        public bool FMI_ActiceFlag { get; set; }
        public List<FeeInstallmentsyearlyDMO> list { get; set; }
    }
}
