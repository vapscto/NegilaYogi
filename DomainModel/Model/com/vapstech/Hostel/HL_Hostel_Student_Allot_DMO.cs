using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Hostel_Student_Allot")]
    public class HL_Hostel_Student_Allot_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HLHSALT_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime HLHSALT_AllotmentDate { get; set; }
        public long ASMAY_Id { get; set; }
        public long HLMH_Id { get; set; }
        public long HLMRCA_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long HRMRM_Id { get; set; }
        public long HLHSALT_NoOfBeds { get; set; }
        public string HLHSALT_AllotRemarks { get; set; }
        public bool HLHSALT_VacateFlg { get; set; }
        public DateTime? HLHSALT_VacatedDate { get; set; }
        public string HLHSALT_VacateRemarks { get; set; }
        public bool HLHSALT_ActiveFlag { get; set; }
        public DateTime? HLHSALT_CreatedDate { get; set; }
        public DateTime? HLHSALT_UpdatedDate { get; set; }
        public long HLHSALT_UpdatedBy { get; set; }
        public long HLHSALT_CreatedBy { get; set; }

    }
}
