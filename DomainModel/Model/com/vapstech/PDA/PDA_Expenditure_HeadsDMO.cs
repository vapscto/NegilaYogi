using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.PDA
{
    [Table("PDA_Expenditure_Heads")]
    public class PDA_Expenditure_HeadsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long PDAEH_Id { get; set; }
        public long PDAE_Id { get; set; }
        public long PDAMH_Id { get; set; }
        public decimal PDAEH_Amount { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string PDAE_Remarks { get; set; }
    }
}
