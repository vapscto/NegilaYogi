using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Master_Fine_Slabs")]
    public class FeeFineSlabDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMFS_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMFS_FineType { get; set; }
        public long FMFS_FromDay { get; set; }
        public long FMFS_ToDay { get; set; }
        public string FMFS_ECSFlag { get; set; }
        public bool FMFS_ActiveFlag { get; set; }
    
    }
}
