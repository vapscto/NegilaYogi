using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeMasterOtherStudentDTO:CommonParamDTO
    {
        public long FMOST_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMOST_StudentName { get; set; }
        public long FMOST_StudentMobileNo { get; set; }
        public string FMOST_StudentEmailId { get; set; }
        public bool FMOST_ActiveFlag { get; set; }
        public Array otherstudentList { get; set; }
        public int count { get; set; }
        public string returnval { get; set; }
    }
}
