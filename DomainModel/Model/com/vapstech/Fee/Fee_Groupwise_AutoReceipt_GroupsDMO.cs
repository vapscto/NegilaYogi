using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Groupwise_AutoReceipt_Groups")]
    public class Fee_Groupwise_AutoReceipt_GroupsDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FGARG_Id { get; set; }       
        public long FGAR_Id { get; set; }
        public long FMG_Id { get; set; }

        public Fee_Groupwise_AutoReceiptDMO Fee_Groupwise_AutoReceiptDMO { get; set; }

    }
}
