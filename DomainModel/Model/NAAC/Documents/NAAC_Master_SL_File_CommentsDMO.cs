using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.NAAC.Documents
{
    [Table("NAAC_Master_SL_File_Comments")]
    public class NAAC_Master_SL_File_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NAACMSLFCO_Id { get; set; }
        public long NAACMSLF_Id { get; set; }
        public string NAACMSLFCO_Remarks { get; set; }
        public long NAACMSLFCO_RemarksBy { get; set; }
        public bool NAACMSLFCO_ActiveFlag { get; set; }
        public long NAACMSLFCO_CreatedBy { get; set; }
        public DateTime NAACMSLFCO_CreatedDate { get; set; }
        public long NAACMSLFCO_UpdatedBy { get; set; }
        public DateTime NAACMSLFCO_UpdatedDate { get; set; }
    }
}
