using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_AC_SParticipation_123_Comments")]
    public class NAAC_AC_SParticipation_123_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NCACSP123C_Id { get; set; }
        public string NCACSP123C_Remarks { get; set; }
        public long? NCACSP123C_RemarksBy { get; set; }
        public string NCACSP123C_StatusFlg { get; set; }
        public bool NCACSP123C_ActiveFlag { get; set; }
        public long? NCACSP123C_CreatedBy { get; set; }
        public DateTime? NCACSP123C_CreatedDate { get; set; }
        public long? NCACSP123C_UpdatedBy { get; set; }
        public DateTime? NCACSP123C_UpdatedDate { get; set; }
        public long NCACSP123_Id { get; set; }
    }
}
