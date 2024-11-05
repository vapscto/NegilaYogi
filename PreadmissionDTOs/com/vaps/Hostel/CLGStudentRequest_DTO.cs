using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Hostel
{
   public class CLGStudentRequest_DTO
    {
        public long HLHSREQC_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime? HLHSREQC_RequestDate { get; set; }
        public long HLMH_Id { get; set; }
        public long HLMRCA_Id { get; set; }
        public long AMCST_Id { get; set; }
        public bool HLHSREQC_EntireRoomReqdFlg { get; set; }
       public bool HLHSREQC_ACRoomReqdFlg { get; set; }
      //   public bool? HLHSREQC_ACRoomFlg { get; set; }
        public bool HLHSREQC_SingleRoomFlg { get; set; }
        //   public bool? HLHSREQC_VegMessFlg { get; set; }
        public bool HLHSREQC_VegMessReqdFlg { get; set; }
       public bool HLHSREQC_NonVegMessReqdFlg { get; set; }
        //public bool? HLHSREQC_NonVegMessFlg { get; set; }
        public string HLHSREQC_Remarks { get; set; }
        public string HLHSREQC_BookingStatus { get; set; }
        public string AMCST_Sex { get; set; }
        public long HLMFTY_Id { get; set; }
        public string HLMFTY_FacilityName { get; set; }
        public bool HLHSREQC_ActiveFlag { get; set; }
        public DateTime? HLHSREQC_CreatedDate { get; set; }
        public DateTime? HLHSREQC_UpdatedDate { get; set; }
        public long? HLHSREQC_CreatedBy { get; set; }
        public long? HLHSREQC_UpdatedBy { get; set; }
        public long ASMAY_Id { get; set; }
        public long UserId { get; set; }
        public string roleflag { get; set; }
        public long roleId { get; set; }

        public bool returnval { get; set; }
        public bool duplicate { get; set; }


        public Array hostel_list { get; set; }
        public Array room_list { get; set; }
        public Array student_wisedata { get; set; }
        public Array all_requestdata { get; set; }
        public Array editlist { get; set; }
        public Array roomdetails { get; set; }
        public Array HRMRM_BedCapacity { get; set; }
        public Array studentList { get; set; }
        public Array room_Details { get; set; }
        public Array facility_list { get; set; }


        public long HLHSALTC_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long? HRMRM_Id { get; set; }
        public DateTime? HLHSALTC_AllotmentDate { get; set; }
        public long HLHSALTC_NoOfBeds { get; set; }
        public string HLHSALTC_AllotRemarks { get; set; }
        public bool? HLHSALTC_VacateFlg { get; set; }
        public DateTime? HLHSALTC_VacatedDate { get; set; }
        public string HLHSALTC_VacateRemarks { get; set; }
        public string returnupdate { get; set; }
        public bool bedcount { get; set; }
        //added by praveen
        public List<CLGStudentRequest_DTO> devicelist12 { get; set; }

    }
}
