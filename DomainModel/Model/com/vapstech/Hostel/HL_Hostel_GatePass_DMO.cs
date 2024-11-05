using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Hostel_Student_Gatepass")]
   public class HL_Hostel_GatePass_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HLHSTGP_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCST_Id { get; set; }     
        public string HLHSTGP_TypeFlg { get; set; }
        public string HLHSTGP_Reason { get; set; }
        public string HLHSTGP_Remarks { get; set; }
        public long? HLHSTGP_CreatedBy { get; set; }
        public long? HLHSTGP_UpdatedBy { get; set; }
        public long? HLHSTGP_TotalDays { get; set; }
        public bool HLHSTGP_ActiveFlg { get; set; }
        public bool? HLHSTGP_ApprovedFlg { get; set; }
        public DateTime? HLHSTGP_GoingOutDate { get; set; }
        public string HLHSTGP_GoingOutTime { get; set; }
        public string HLHSTGP_ComingBackTime { get; set; }
        public DateTime? HLHSTGP_CameBackDate { get; set; }
        public String HLHSTGP_CameBackTime { get; set; }
        public DateTime? HLHSTGP_ComingBackDate { get; set; }
        public DateTime? HLHSTGP_CreatedDate { get; set; }
        public DateTime? HLHSTGP_UpdatedDate { get; set; }
    }
}
