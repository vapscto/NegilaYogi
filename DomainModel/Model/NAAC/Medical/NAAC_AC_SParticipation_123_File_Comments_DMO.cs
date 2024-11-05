using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_AC_SParticipation_123_File_Comments")]
    public class NAAC_AC_SParticipation_123_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACSP123FC_Id { get; set; }
        public string NCACSP123FC_Remarks { get; set; }
        public long NCACSP123FC_RemarksBy { get; set; }
        public bool NCACSP123FC_ActiveFlag { get; set; }
        public long? NCACSP123FC_CreatedBy { get; set; }
        public DateTime? NCACSP123FC_CreatedDate { get; set; }
        public long? NCACSP123FC_UpdatedBy { get; set; }
        public DateTime? NCACSP123FC_UpdatedDate { get; set; }
        public string NCACSP123FC_StatusFlg { get; set; }
        public long NCACSP123F_Id { get; set; }

    }
}
