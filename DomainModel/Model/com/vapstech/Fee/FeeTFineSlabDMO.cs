using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_T_Fine_Slabs")]
    public class FeeTFineSlabDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FTFS_Id { get; set; }
        public long FMFS_Id { get; set; }
        public long FMA_Id { get; set; }
        public string FTFS_FineType { get; set; }
        public decimal FTFS_Amount { get; set; }

        public DateTime? FTFS_Date { get; set; }
    }
}
