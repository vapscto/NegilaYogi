using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_T_Installment")]
    public class FeeInstallmentsyearlyDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FTI_Id { get; set; }
        public long MI_ID{ get; set;}
        public long FMI_Id { get; set; }
        public string FTI_Name { get; set; }
        public bool? FTI_Active { get; set; }

    }
}
