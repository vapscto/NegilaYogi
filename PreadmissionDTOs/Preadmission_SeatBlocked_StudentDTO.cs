using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class Preadmission_SeatBlocked_StudentDTO : CommonParamDTO
    {
        public long PASBS_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long PASR_Id { get; set; }
        public DateTime PASBS_Date { get; set; }
        public long IVRMSTAUL_Id { get; set; }

        public Array SeatBlockedList { get; set; }
        public Array InstuteDropDown { get; set; }
        public Array YearDropdown { get; set; }
        public Array RegstudentdropDown { get; set; }
        public Array staffUserDropdown { get; set; }

        public string InstituteName { get; set; }
        public string studentName { get; set; }
        public string Year { get; set; }
        public string staff { get; set; }
        public bool returnVal { get; set; }
        public int count { get; set; }
        public string message { get; set; }
    }
}
