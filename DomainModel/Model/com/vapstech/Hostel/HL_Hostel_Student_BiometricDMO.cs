using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Hostel_Student_Biometric")]
    public class HL_Hostel_Student_BiometricDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HLHSTBIO_Id { get; set; }
       public long MI_Id { get; set; }
       public long AMCST_Id { get; set; }
       public long? HLHSTBIO_CreatedBy { get; set; }
       public long? HLHSTBIO_UpdatedBy { get; set; }
       public DateTime? HLHSTBIO_CreatedDate { get; set; }
       public DateTime? HLHSTBIO_PunchDate { get; set; }
       public DateTime? HLHSTBIO_UpdatedDate { get; set; }
       public bool? HLHSTBIO_ActiveFlg { get; set; }

    }
}
