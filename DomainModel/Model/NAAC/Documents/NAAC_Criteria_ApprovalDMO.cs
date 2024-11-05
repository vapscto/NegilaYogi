using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.NAAC.Documents
{
    [Table("NAAC_Criteria_Approval")]
    public class NAAC_Criteria_ApprovalDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NAC_Id { get; set; }
        public long MI_Id { get; set; }
        public string NAACSL_SLNo { get; set; }
        public long NAC_CycleId { get; set; }
        public string NAC_Flag { get; set; }
        public long NAC_UserId { get; set; }
        public DateTime? NAC_CreatedDate { get; set; }
        public DateTime? NAC_UpdateDate { get; set; }
    }
}
