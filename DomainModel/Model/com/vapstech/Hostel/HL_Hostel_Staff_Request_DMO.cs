using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Hostel_Staff_Request")]
    public class HL_Hostel_Staff_Request_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HLHSTREQ_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime HLHSTREQ_RequestDate { get; set; }
        public long HLMH_Id { get; set; }
        public long HLMRCA_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool HLHSTREQ_ACRoomReqdFlg { get; set; }
        public bool HLHSTREQ_EntireRoomReqdFlg { get; set; }
        public bool HLHSTREQ_VegMessReqdFlg { get; set; }
        public bool HLHSTREQ_NonVegMessReqdFlg { get; set; }
        public string HLHSTREQ_Remarks { get; set; }
        public string HLHSTREQ_BookingStatus { get; set; }
        public bool HLHSTREQ_ActiveFlag { get; set; }
        public DateTime? HLHSTREQ_CreatedDate { get; set; }
        public DateTime? HLHSTREQ_UpdatedDate { get; set; }
        public long? HLHSTREQ_CreatedBy { get; set; }
        public long? HLHSTREQ_UpdatedBy { get; set; }

        public List<HL_Hostel_Staff_Request_Confirm_DMO> HL_Hostel_Staff_Request_Confirm_DMO { get; set; }

    }
}
