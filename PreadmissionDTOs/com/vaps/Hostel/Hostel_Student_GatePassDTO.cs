using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Hostel
{
    public class Hostel_Student_GatePassDTO : CommonParamDTO
    {

        public DateTime? CameBackDate { get; set; }
        public string Comingbacktime { get; set; }

        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public long LogInUserId { get; set; }



        public DateTime? HLHSTGP_GoingOutDate { get; set; }
        public string HLHSTGP_GoingOutTime { get; set; }
        public string HLHSTGP_ComingBackTime { get; set; }
        public DateTime? HLHSTGP_ComingBackDate { get; set; }
        public DateTime? HLHSTGP_CameBackDate { get; set; }
        public string HLHSTGP_CameBackTime { get; set; }
        public DateTime? HLHSTGP_CreatedDate { get; set; }
        public DateTime? HLHSTGP_UpdatedDate { get; set; }


        public string retrunMsg { get; set; }


        public string RoleType { get; set; }
        public long roleid { get; set; }
        public String HLHSTGPAPP_Remarks { get; set; }
        public long HLHSTGPAPP_Id { get; set; }

        public long ASMAY_Id { get; set; }
        public long HLHSTGP_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long Student_id { get; set; }
        public string HLHSTGP_TypeFlg { get; set; }
        public string HLHSTGP_Reason { get; set; }
        public string HLHSTGP_Remarks { get; set; }
        public long? HLHSTGP_CreatedBy { get; set; }
        public long? HLHSTGP_UpdatedBy { get; set; }
        public long? HLHSTGP_TotalDays { get; set; }
        public bool? HLHSTGP_ActiveFlg { get; set; }
        public bool? HLHSTGP_ApprovedFlg { get; set; }
        public bool? AMCST_ActiveFlag { get; set; }
        public Array gridlistdata { get; set; }
        public Array savedata { get; set; }
        public Array editdata { get; set; }
        public Array griddisplay { get; set; }
        public Array approved { get; set; }
        public Array employees { get; set; }
        public Array getstudent { get; set; }
        public Array admingatepassapplylist { get; set; }
        public string rdbbutton { get; set; }
        public string Status_Type { get; set; }
        public Array approvalReport { get; set; }
        public String HLHSTGPAPP_Status { get; set; }
        public long yearid { get; set; }
        public monthdto[] MONTHID { get; set; }
        public employeeDTO[] AMCSTId { get; set; }
        public class monthdto
        {
            public long IVRM_Month_Id { get; set; }
        }
        public class employeeDTO
        {
            public long AMCST_Id { get; set; }
        }


    }

}




