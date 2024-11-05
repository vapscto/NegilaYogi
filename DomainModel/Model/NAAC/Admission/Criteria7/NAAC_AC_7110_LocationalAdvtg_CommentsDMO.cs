using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria7
{
    [Table("NAAC_AC_7110_LocationalAdvtg_Comments")]
    public class NAAC_AC_7110_LocationalAdvtg_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC7110LOCADVTGC_Id { get; set; }
        public long NCAC7110LOCADVTG_Id { get; set; }
        public long NCAC7110LOCADVTGC_RemarksBy { get; set; }
        public string NCAC7110LOCADVTGC_Remarks { get; set; }
        public string NCAC7110LOCADVTGC_StatusFlg { get; set; }
        public bool NCAC7110LOCADVTGC_ActiveFlag { get; set; }
        public long NCAC7110LOCADVTGC_CreatedBy { get; set; }
        public long NCAC7110LOCADVTGC_UpdatedBy { get; set; }
        public DateTime? NCAC7110LOCADVTGC_CreatedDate { get; set; }
        public DateTime? NCAC7110LOCADVTGC_UpdatedDate { get; set; }

    }
}
