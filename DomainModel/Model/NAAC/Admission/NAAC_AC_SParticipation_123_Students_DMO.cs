using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_SParticipation_123_Students")]
    public class NAAC_AC_SParticipation_123_Students_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACSP123S_Id { get; set; }
        public long NCACSP123_Id { get; set; }
        public long AMCST_Id { get; set; }
        public bool NCACSP123S_ActiveFlg { get; set; }
        public long NCACSP123S_CreatedBy { get; set; }
        public long NCACSP123S_UpdatedBy{ get; set; }
        public DateTime? NCACSP123S_CreatedDate{ get; set; }
        public DateTime? NCACSP123S_UpdatedDate { get; set; }

    }
}
