using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.NAAC
{
    [Table("NAAC_Master_Trust_Cycle")]
    public class NAAC_Master_Trust_CycleDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMATC_Id { get; set; }
        public long MO_Id { get; set; }
        public string NCMATC_NAACCycle { get; set; }
        public DateTime? NCMATC_FromDate { get; set; }
        public DateTime? NCMATC_TODate { get; set; }
        public bool NCMATC_ActiveFlg { get; set; }
        public long NCMATC_CreatedBy { get; set; }
        public long NCMATC_UpdatedBy { get; set; }
        public DateTime? NCMATC_CreatedDate { get; set; }
        public DateTime? NCMATC_UpdatedDate { get; set; }
        public int NCMATC_Order { get; set; }
    }
}
