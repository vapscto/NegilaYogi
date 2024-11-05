using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Hostel_Staff_Request_Confirm")]
    public class HL_Hostel_Staff_Request_Confirm_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HLHSTREQC_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime HLHSTREQC_RequestDate { get; set; }
        public long HRME_Id { get; set; }       
        public long HLMH_Id { get; set; }
        public long HLMRCA_Id { get; set; }    
        public bool HLHSTREQC_ACRoomFlg { get; set; }
        public bool HLHSTREQC_SingleRoomFlg { get; set; }
        public bool HLHSTREQC_VegMessFlg { get; set; }
        public bool HLHSTREQC_NonVegMessFlg { get; set; }
        public string HLHSTREQC_Remarks { get; set; }
        public string HLHSTREQC_BookingStatus { get; set; }
        public bool HLHSTREQC_ActiveFlag { get; set; }
        public DateTime? HLHSTREQC_CreatedDate { get; set; }
        public DateTime? HLHSTREQC_UpdatedDate { get; set; }
        public long HLHSTREQC_CreatedBy { get; set; }
        public long HLHSTREQC_UpdatedBy { get; set; }
        public long HLHSTREQ_Id { get; set; }
        public long? HRMRM_Id { get; set; }

    }
}
