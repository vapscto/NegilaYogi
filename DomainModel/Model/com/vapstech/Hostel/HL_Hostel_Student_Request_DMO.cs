using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Hostel_Student_Request")]
    public class HL_Hostel_Student_Request_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HLHSREQ_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime HLHSREQ_RequestDate { get; set; }
        public long HLMH_Id { get; set; }
        public long HLMRCA_Id { get; set; }
        public long AMST_Id { get; set; }
        public bool HLHSREQ_ACRoomFlg { get; set; }
        public bool HLHSREQ_SingleRoomFlg { get; set; }
        public bool HLHSREQ_VegMessFlg { get; set; }
        public bool HLHSREQ_NonVegMessFlg { get; set; }
        public string HLHSREQ_Remarks { get; set; }
        public string HLHSREQ_BookingStatus { get; set; }
        public bool HLHSREQ_ActiveFlag { get; set; }
        public DateTime? HLHSREQ_CreatedDate { get; set; }
        public DateTime? HLHSREQ_UpdatedDate { get; set; }
        public long HLHSREQ_CreatedBy { get; set; }
        public long HLHSREQ_UpdatedBy { get; set; }

        public List<HL_Hostel_Student_Request_Confirm_DMO> HL_Hostel_Student_Request_Confirm_DMO { get; set; }

    }
}
