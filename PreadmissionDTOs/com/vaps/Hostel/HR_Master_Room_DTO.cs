using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace PreadmissionDTOs.com.vaps.Hostel
{
    public class HR_Master_Room_DTO : CommonParamDTO
    {
        public long HRMRM_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long HLMH_Id { get; set; }
        public long HLMF_Id { get; set; }
        public string HRMRM_RoomNo { get; set; }
        public string HRMRM_SharingFlg { get; set; }
        public string HRMRM_ACFlg { get; set; }
        public long HRMRM_BedCapacity { get; set; }
        public string HRMRM_RoomDesc { get; set; }
        public bool HRMRM_RoomForStudentFlg { get; set; }
        public bool HRMRM_RoomForStaffFlg { get; set; }
        public bool HRMRM_RoomForGuestFlg { get; set; }
        public bool HRMRM_ActiveFlag { get; set; }
        public long HRMRM_CreatedBy { get; set; }
        public long HRMRM_UpdatedBy { get; set; }
        public string HLMH_Name { get; set; }
        public string HRMF_FloorName { get; set; }
        public string HLMFTY_FacilityName { get; set; }
        public long HRMRMF_Id { get; set; }
        public long HLMFTY_Id { get; set; }
        public long HLMRCA_Id { get; set; }
        public bool HRMRMF_ActiveFlg { get; set; }        
        public string HLMRCA_RoomCategory { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array hostellist { get; set; }
        public Array floor_list { get; set; }
        public Array categorydetails { get; set; }
        public Array edit_Roomdatalist { get; set; }
        public Array get_room_list { get; set; }
        public Array facilities_list { get; set; }
        public Array editfaclist { get; set; }
        public Array get_MappedfacilityforRoom { get; set; }
        public Array room_catlist { get; set; }

        public HR_Master_Room_DTO[] selected_facilities {get;set;}

    }
}
