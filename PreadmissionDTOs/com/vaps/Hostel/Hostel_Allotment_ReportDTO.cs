using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Hostel
{
  public  class Hostel_Allotment_ReportDTO
    {
        //=========student
        public long HLHSALT_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime HLHSALT_AllotmentDate { get; set; }
        public long ASMAY_Id { get; set; }
        public long HLMH_Id { get; set; }
        public long HLMF_Id { get; set; }
        public long HLMRCA_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long HRMRM_Id { get; set; }      
        public string HLHSALT_AllotRemarks { get; set; }
        public bool HLHSALT_VacateFlg { get; set; }
        public DateTime? HLHSALT_VacatedDate { get; set; }
        public string HLHSALT_VacateRemarks { get; set; }
        public bool HLHSALT_ActiveFlag { get; set; }

        //=========stafff
        public long HLHSTALT_Id { get; set; }
        public DateTime HLHSTALT_AllotmentDate { get; set; }
        public long HRME_Id { get; set; }   
        public long HLHSTALT_NoOfBeds { get; set; }
        public string HLHSTALT_AllotRemarks { get; set; }
        public bool HLHSTALT_VacateFlg { get; set; }
        public DateTime HLHSTALT_VacatedDate { get; set; }
        public string HLHSTALT_VacateRemarks { get; set; }
        public bool HLHSTALT_ActiveFlag { get; set; }

        //============guest
        public long HLHGSTALT_Id { get; set; }    
        public long HLHGSTALT_AllotmentDate { get; set; }
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

        public Array yearlist { get; set; }
        public Array hostellist { get; set; }
        public Array griddata { get; set; }
        public Array floorlist { get; set; }
        public Array roomcategorylist { get; set; }
        public Array getstudentalloteddata { get; set; }
        public string type { get; set; }
        public string frmdate { get; set; }
        public string todate { get; set; }
        public string HLMH_Name { get; set; }
        public string institution_flag { get; set; }
        public Room_Category_DTO_Temp[] Room_Category_DTO_Temp { get; set; }
        public Floor_DTO_Temp[] Floor_DTO_Temp { get; set; }
    }

    public class Room_Category_DTO_Temp
    {
        public long HLMRCA_Id { get; set; }
        public string HLMRCA_RoomCategory { get; set; }
        public long? HLMRCA_MaxCapacity { get; set; }
    }

    public class Floor_DTO_Temp
    {
        public long HLMF_Id { get; set; }
        public long HLMH_Id { get; set; }
        public string HRMF_FloorName { get; set; }
        public long? HRMF_TotalRooms { get; set; }
    }
}
