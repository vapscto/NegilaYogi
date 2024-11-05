using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.NAAC.Documents
{
    [Table("NAAC_Master_SL_Link")]
    public class NAAC_Master_SL_LinkDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NAACMSLLK_Id { get; set; }
        public long NAACMSL_Id { get; set; }
        public string NAACMSLLK_LinkName { get; set; }
        public string NAACMSLLK_LinkRemarks { get; set; }
        public bool NAACMSLLK_ActiveFlag { get; set; }
        public long NAACMSLLK_CreatedBy { get; set; }
        public DateTime NAACMSLLK_CreatedDate { get; set; }
        public long NAACMSLLK_UpdatedBy { get; set; }
        public DateTime NAACMSLLK_UpdatedDate { get; set; }
    }
}
