using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Fees
{
    public class Fee_Master_College_OtherStudentsDTO
    {
        public long FMCOST_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMCOST_StudentName { get; set; }
        public long FMCOST_StudentMobileNo { get; set; }
        public string FMCOST_StudentEmailId { get; set; }
        public bool FMCOST_ActiveFlag { get; set; }

        public DateTime FMCOST_CreatedDate { get; set; }
        public DateTime FMCOST_UpdatedDate { get; set; }
        public long FMCOST_CreatedBy { get; set; }
        public long FMCOST_UpdatedBy { get; set; }

        public String returnval { get; set; }

        public long User_Id { get; set; }

        public Array otherstudentList { get; set; }
        public int count { get; set; }
    }
}
