using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Hostel_Guest_Allot")]
    public class HL_Hostel_Guest_Allot_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HLHGSTALT_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime HLHGSTALT_AllotmentDate { get; set; }
        public long HLMH_Id { get; set; }
        public long HLMRCA_Id { get; set; }
        public long HRMRM_Id { get; set; }
        public string HLHGSTALT_GuestName { get; set; }
        public long HLHGSTALT_GuestPhoneNo { get; set; }
        public string HLHGSTALT_GuestEmailId { get; set; }
        public string HLHGSTALT_GuestAddress { get; set; }
        public string HLHGSTALT_GuestPhoto { get; set; }
        public string HLHGSTALT_AddressProof { get; set; }
        public long HLHGSTALT_NoOfBeds { get; set; }
        public string HLHGSTALT_AllotRemarks { get; set; }
        public bool HLHGSTALT_VacateFlg { get; set; }
        public DateTime? HLHGSTALT_VacatedDate { get; set; }
        public string HLHGSTALT_VacateRemarks { get; set; }
        public bool HLHGSTALT_ActiveFlag { get; set; }
        public DateTime? HLHGSTALT_CreatedDate { get; set; }
        public DateTime? HLHGSTALT_UpdatedDate { get; set; }
        public long HLHGSTALT_UpdatedBy { get; set; }
        public long HLHGSTALT_CreatedBy { get; set; }
    }
}
