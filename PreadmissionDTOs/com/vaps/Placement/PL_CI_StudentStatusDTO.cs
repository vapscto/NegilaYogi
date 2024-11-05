using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Placement
{
    public class PL_CI_StudentStatusDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }

        public long MI_Id { get; set; }
        public long roleid { get; set; }
        public long UserId { get; set; }
        public long studentid { get; set; }
        public Array studentname { get; set; }
        public Array editdata { get; set; }
        public Array tablegrid { get; set; }
        public string sname { get; set; }


        public long PLCISCHCOMJTSTS_Id { get; set; }
        public long PLCISCHCOMJTST_Id { get; set; }
        public string PLCISCHCOMJTSTS_InterviewRound { get; set; }
        public string PLCISCHCOMJTSTS_Marks { get; set; }
        public string PLCISCHCOMJTSTS_TestType { get; set; } 
        public string PLCISCHCOMJTSTS_Remarks { get; set; }
        public bool PLCISCHCOMJTSTS_SelectedFlg { get; set; }
        public bool PLCISCHCOMJTSTS_ActiveFlag { get; set; }
        public DateTime PLCISCHCOMJTSTS_CreatedDate { get; set; }
        public long PLCISCHCOMJTSTS_CreatedBy { get; set; }
        public DateTime PLCISCHCOMJTSTS_UpdatedDate { get; set; }
        public long PLCISCHCOMJTSTS_UpdatedBy { get; set; }
    }
}
