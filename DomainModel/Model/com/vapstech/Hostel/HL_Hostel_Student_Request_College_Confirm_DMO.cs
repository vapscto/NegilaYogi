using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Hostel_Student_Request_College_Confirm")]
    public class HL_Hostel_Student_Request_College_Confirm_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HLHSREQCC_Id { get; set; }
        public long? HLHSREQC_Id { get; set; }
        public DateTime? HLHSREQC_Date { get; set; }
        public long HRME_Id { get; set; }
        public long HLMH_Id { get; set; }
        public long HLMRCA_Id { get; set; }
        public bool? HLHSREQCC_ACRoomFlg { get; set; }
        public bool? HLHSREQCC_SingleRoomFlg { get; set; }
        public bool? HLHSREQCC_VegMessFlg { get; set; }
        public bool? HLHSREQCC_NonVegMessFlg { get; set; }
        public string HLHSREQCC_Remarks { get; set; }
        public string HLHSREQCC_BookingStatus { get; set; }
        public long? HRMRM_Id { get; set; }
        public bool? HLHSREQCC_ActiveFlag { get; set; }
        public DateTime? HLHSREQCC_CreatedDate { get; set; }
        public DateTime? HLHSREQCC_UpdatedDate { get; set; }
        public long? HLHSREQCC_CreatedBy { get; set; }
        public long? HLHSREQCC_UpdatedBy { get; set; }

    }
}
