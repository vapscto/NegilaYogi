using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace PreadmissionDTOs.com.vaps.Hostel
{
    public class HR_Master_Floor_DTO : CommonParamDTO
    {
        public long HLMF_Id { get; set; }
        public long MI_Id { get; set; }
        public long HLMH_Id { get; set; }
        public string HRMF_FloorName { get; set; }
        public long HRMF_TotalRooms { get; set; }
        public string HRMF_FloorDesc { get; set; }
        public bool HRMF_ActiveFlag { get; set; }
        public long HRMF_CreatedBy { get; set; }
        public long HRMF_UpdatedBy { get; set; }


        public string HLMH_Name { get; set; }
        public long HLMFTY_Id { get; set; }
        public long UserId { get; set; }
        public string HLMFTY_FacilityName { get; set; }

        public Array grid_Alldataforfloor { get; set; }
        public Array facilty_list { get; set; }
        public Array houstel_list { get; set; }
        public Array facilities_list { get; set; }
        public Array edit_floorlist { get; set; }
        public Array mappedfacilitylist { get; set; }
        public bool returnval { get; set; }   

        public bool duplicate { get; set; }
        public Array get_MappedfacilityforRoom { get; set; }

        public Array floor_lists { get; set; }
        public selected_facilities[] selected_facilities {get;set;}


    }
    public class selected_facilities
    {
        public long HLMFTY_Id { get; set; }
    }

}