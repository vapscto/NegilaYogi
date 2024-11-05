using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Hostel_Student_Allot_College")]
    public class HL_Hostel_Student_Allot_College_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HLHSALTC_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime? HLHSALTC_AllotmentDate { get; set; }
        public long ASMAY_Id { get; set; }
        public long HLMH_Id { get; set; }
        public long HLMRCA_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long HRMRM_Id { get; set; }
       // public long HLHSALTC_NoOfBeds { get; set; }
        public string HLHSALTC_AllotRemarks { get; set; }
        public bool? HLHSALTC_VacateFlg { get; set; }
        public DateTime? HLHSALTC_VacatedDate { get; set; }
        public string HLHSALTC_VacateRemarks { get; set; }
        public bool? HLHSALTC_ActiveFlag { get; set; }    
        public DateTime? HLHSALTC_CreatedDate { get; set; }
        public DateTime? HLHSALTC_UpdatedDate { get; set; }
        public long? HLHSALTC_UpdatedBy { get; set; }
        public long? HLHSALTC_CreatedBy { get; set; }

        public bool? HLHSALTC_EntireRoomReqdFlg { get; set; }
    }
}
