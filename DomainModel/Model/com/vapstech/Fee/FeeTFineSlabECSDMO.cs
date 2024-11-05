using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_T_Fine_Slabs_ECS")]
    public class FeeTFineSlabECSDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FTFSE_Id { get; set; }
        public long FMFS_Id { get; set; }
        public long FMA_Id { get; set; }
        public string FTFSE_FineType { get; set; }
        public decimal FTFSE_Amount { get; set; }

    }
}
