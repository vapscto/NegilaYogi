using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_T_Installment")]
    public class Clg_Fee_Installments_Yearly_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FTI_Id { get; set; }
        public long MI_ID { get; set; }
        public long FMI_Id { get; set; }
        public string FTI_Name { get; set; }
    }
}
