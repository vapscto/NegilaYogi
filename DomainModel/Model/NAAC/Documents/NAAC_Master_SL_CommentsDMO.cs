using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.NAAC.Documents
{
    [Table("NAAC_Master_SL_Comments")]
    public class NAAC_Master_SL_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NAACMSLCO_Id { get; set; }
        public long NAACSL_Id { get; set; }
        public long MI_Id { get; set; }
        public string NAACMSLCO_Remarks { get; set; }
        public long NAACMSLCO_RemarksBy { get; set; }
        public bool NAACMSLCO_ActiveFlag { get; set; }
        public long NAACMSLCO_CreatedBy { get; set; }
        public DateTime NAACMSLCO_CreatedDate { get; set; }
        public long NAACMSLCO_UpdatedBy { get; set; }
        public DateTime NAACMSLCO_UpdatedDate { get; set; }
    }
}
