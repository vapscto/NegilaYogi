using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Hostel_Student_Gatepass_Approval")]
    public class Hostel_Student_GatePass_ApprovalDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HLHSTGPAPP_Id { get; set; }
        public long HLHSTGP_Id { get; set; }
        public long Id { get; set; }
        public String HLHSTGPAPP_Remarks { get; set; }
        public String HLHSTGPAPP_Status { get; set; }
        public bool HLHSTGPAPP_ActiveFlg { get; set; }
        public long? HLHSTGPAPP_CreatedBy { get; set; }
        public long? HLHSTGPAPP_UpdatedBy { get; set; }
        public DateTime? HLHSTGPAPP_CreatedDate { get; set; }
        public DateTime? HLHSTGPAPP_UpdatedDate { get; set; }
    }
}
