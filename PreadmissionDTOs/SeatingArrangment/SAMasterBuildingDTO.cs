using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.SeatingArrangment
{
    public class SAMasterBuildingDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }
        public DateTime Createdate { get; set; }

        // Master Building
        public long ESABLD_Id { get; set; }
        public string ESABLD_BuildingName { get; set; }
        public string ESABLD_BuildingDesc { get; set; }
        public bool ESABLD_ActiveFlg { get; set; }
        public Array getbuildingdetails { get; set; }
        public Array geteditbuildingdetails { get; set; }

        // Master Floor
        public long ESAFLR_Id { get; set; }
        public string ESAFLR_FloorName { get; set; }
        public string ESAFLR_FloorDesc { get; set; }
        public bool ESAFLR_ActiveFlg { get; set; }
        public Array getfloordetails { get; set; }
        public Array getmasterbuilding { get; set; }
        public Array geteditfloordetails { get; set; }

        // Master Room
        public long ESAROOM_Id { get; set; }
        public string ESAROOM_RoomName { get; set; }
        public string ESAROOM_RoomDesc { get; set; }
        public long? ESAROOM_RoomMaxCapacity { get; set; }
        public string ESAROOM_RoomTypeFlg { get; set; }
        public long ESAROOM_MaxNoOfRows { get; set; }
        public long ESAROOM_MaxNoOfColumns { get; set; }
        public long ESAROOM_BenchCapacity { get; set; }
        public bool ESAROOM_ActiveFlg { get; set; }
        public Array getroombuilding { get; set; }
        public Array getbuildingfloordetails { get; set; }
        public Array getroomdetails { get; set; }
        public Array geteditroombuilding { get; set; }
        public Array geteditbuildingfloordetails { get; set; }
        public Array geteditroomdetails { get; set; }

        // Master University Exam
        public long ESAUE_Id { get; set; }
        public string ESAUE_ExamName { get; set; }
        public string ESAUE_ExamCode { get; set; }
        public int ESAUE_ExamOrder { get; set; }
        public bool ESAUE_ActiveFlag { get; set; }
        public Array getuniversityexamdetails { get; set; }
        public Array getuniversityexamorderdetails { get; set; }
        public Array getedituniversityexamdetails { get; set; }
        public Temp_OrderUpdate[] Temp_OrderUpdate { get; set; }

        // Master Duty Type
        public long ESAALLSTADTP_Id { get; set; }
        public string ESAALLSTADTP_DTName { get; set; }
        public bool ESAALLSTADTP_ActiveFlag { get; set; }
        public Array getdutytypedetails { get; set; }
        public Array geteditdutytypedetails { get; set; }

        // Master Time Slot
        public long ESAESLOT_Id { get; set; }
        public string ESAESLOT_SlotName { get; set; }
        public string ESAESLOT_StartTime { get; set; }
        public string ESAESLOT_EndTime { get; set; }
        public bool ESAESLOT_ActiveFlg { get; set; }
        public Array getslottimedetails { get; set; }
        public Array geteditslottimedetails { get; set; }
    }

    public class Temp_OrderUpdate
    {
        public long ESAUE_Id { get; set; }
        public int ESAUE_ExamOrder { get; set; }
    }
}
