using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Hostel_Student_Request_College")]
    public class HL_Hostel_Student_Request_College_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HLHSREQC_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime? HLHSREQC_RequestDate { get; set; }
        public long HLMH_Id { get; set; }
        public long HLMRCA_Id { get; set; }
        public long AMCST_Id { get; set; }
        public bool? HLHSREQC_ACRoomReqdFlg { get; set; }
        public bool? HLHSREQC_EntireRoomReqdFlg { get; set; }
        public bool? HLHSREQC_VegMessReqdFlg { get; set; }
        public bool? HLHSREQC_NonVegMessReqdFlg { get; set; }
        public string HLHSREQC_Remarks { get; set; }
        public string HLHSREQC_BookingStatus { get; set; }
        public bool? HLHSREQC_ActiveFlag { get; set; }
        public DateTime? HLHSREQC_CreatedDate { get; set; }
        public DateTime? HLHSREQC_UpdatedDate { get; set; }
        public long? HLHSREQC_CreatedBy { get; set; }
        public long? HLHSREQC_UpdatedBy { get; set; }
        public long? HRMRM_Id { get; set; }
    }
}
