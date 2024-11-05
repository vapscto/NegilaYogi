using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_T_Due_Date")]
    public class FeeTDueDateRegularDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FTDD_Id { get; set; }
        public long FMA_Id { get; set; }
        public string FTDD_Day { get; set; }
        public string FTDD_Month { get; set; }

    }
}
