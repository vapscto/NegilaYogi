using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_Master_Installment")]
    public class Clg_Fee_Installment_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMI_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMI_Name { get; set; }
        public long FMI_No_Of_Installments { get; set; }
        public string FMI_Installment_Type { get; set; }
        public bool FMI_ActiceFlag { get; set; }
        public List<Clg_Fee_Installments_Yearly_DMO> list { get; set; }
    }
}
