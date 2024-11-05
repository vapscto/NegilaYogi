using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Hostel
{
    public class Hostel_Student_InOutDTO
    {
        public long MI_Id { get; set; }
        public long roleid { get; set; }
        public long AMCST_Id { get; set; }
        public string AMCST_FirstName { get; set; }
        public DateTime empdate { get; set; }
        public DateTime? HLHSTBIO_PunchDate { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }


      
        public Array studentlist1 { get; set; }
        public Array studentlist { get; set; }
        public Array employeelist { get; set; }
        public Array viewlist { get; set; }
        public long ASMCL_Id { get; set; }
        public long HLHSTBIO_Id { get; set; }
        public long AMST_Id { get; set; }
        public long amstid { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string inoutflag { get; set; }
        public string optionflag { get; set; }
        public long ASMS_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public string AMST_FirstName { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public string HLHSTBIOD_PunchTime { get; set; }
        public string HLHSTBIOD_InOutFlg { get; set; }
        public employeeDTO[] AMCSTId { get; set; }
        public class employeeDTO
        {
            public long AMCST_Id { get; set; }
        }

    }
}
