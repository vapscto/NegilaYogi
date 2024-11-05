using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_T_Due_Date_ECS")]
    public class FeeTDueDateECSDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FTDDE_Id { get; set; }
        public long FMA_Id { get; set; }
        public string FTDDE_Day { get; set; }
        public string FTDDE_Month { get; set; }
    }
}
