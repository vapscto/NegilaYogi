using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace PreadmissionDTOs.com.vaps.VMS.Training
{
    public class HR_Master_Floor_DTO : CommonParamDTO
    {
        public long HRMF_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMB_Id { get; set; }
        public string HRMF_FloorName { get; set; }
        public bool HRMF_ActiveFlag { get; set; }
        public long HRMF_CreatedBy { get; set; }
        public long HRMF_UpdatedBy { get; set; }
        public long userId { get; set; }
        public bool returnval { get; set; }
        public string returnvalue { get; set; }
        public string HRMB_BuildingName { get; set; }
        public Array floor_list { get; set; }
        public Array building_list { get; set; }
        public Array get_MappedfacilityforRoom { get; set; }

        public Array floor_lists { get; set; }
        public Array hostel_list { get; set; }
        public Array floor { get; set; }
        public Array room { get; set; }
        public long HLMB_Id { get; set; }
        public bool HLMB_BedSheetFlg { get; set; }
        public bool HLMB_MattressFlg { get; set; }
        public bool HLMB_PillowFlg { get; set; }
        public bool HLMB_StudyTableFlg { get; set; }
        public bool HLMB_LampFlg { get; set; }
        public string HLMB_BedName { get; set; }
        public long HLMH_Id { get; set; }
        public string HLMH_Name { get; set; }
        public string HRMRM_RoomNo { get; set; }
        public long HLMF_Id { get; set; }
        public long HRMRM_Id { get; set; }
        public String retrunMsg { get; set; }




    }
}