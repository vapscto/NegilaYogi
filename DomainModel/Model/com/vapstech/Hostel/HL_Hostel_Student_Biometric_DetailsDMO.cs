using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Hostel_Student_Biometric_Details")]
    public class HL_Hostel_Student_Biometric_DetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HLHSTBIOD_Id { get; set; }
        public long HLHSTBIO_Id { get; set; }
        public long? HLHSTBIOD_CreatedBy { get; set; }
        public long? HLHSTBIOD_UpdatedBy { get; set; }
        public string HLHSTBIOD_PunchTime { get; set; }
        public string HLHSTBIOD_InOutFlg { get; set; }
        public bool HLHSTBIOD_ManualEntryFlg { get; set; }
        public bool HLHSTBIOD_ActiveFlg { get; set; }
        public DateTime? HLHSTBIOD_CreatedDate { get; set; }
        public DateTime? HLHSTBIOD_UpdatedDate { get; set; }

    }
}
