using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Hostel
{
   public class HostelAllotForGuest_DTO
    {

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
        public DateTime HLHGSTALT_VacatedDate { get; set; }
        public string HLHGSTALT_VacateRemarks { get; set; }
        public bool HLHGSTALT_ActiveFlag { get; set; }
        public DateTime? HLHGSTALT_CreatedDate { get; set; }
        public DateTime? HLHGSTALT_UpdatedDate { get; set; }
        public long HLHGSTALT_UpdatedBy { get; set; }
        public long HLHGSTALT_CreatedBy { get; set; }

        public long UserId { get; set; }
        public Array hstllist { get; set; }
        public Array categorylist { get; set; }
        public Array roomlist { get; set; }
        public Array alldata1 { get; set; }
        public Array editlist { get; set; }
        public bool returnval { get; set; }
        public bool ret { get; set; }
        public string msg { get; set; }
        public string HLMH_Name { get; set; }
        public string HRMRM_RoomNo { get; set; }
        public string HLMRCA_RoomCategory { get; set; }

    }
}
