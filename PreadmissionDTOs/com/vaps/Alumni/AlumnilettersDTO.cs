using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Alumni
{
    public class AlumnilettersDTO
    {
        public Array allacademicyear { get; set; }
        public Array newuser1 { get; set; }
        public Array newuser2 { get; set; }
        public Array classlist { get; set; }
        public Array stu_name { get; set; }
        public Array SearchstudentDetails { get; set; }
        public Array searchstudentDetails2 { get; set; }
        public Array logopath { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long AMST_ID { get; set; }
        public long ALMST_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public long roleId { get; set; }
        public AlumnilettersDTO[] studlistdata { get; set; }
        public string logoname { get; set; }
        public Array searchResult { get; set; }
        public int count { get; set; }
    }
}
