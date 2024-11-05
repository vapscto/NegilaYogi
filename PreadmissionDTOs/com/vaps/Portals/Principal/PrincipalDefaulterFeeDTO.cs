using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Principal
{
    public class PrincipalDefaulterFeeDTO
    {

        public int EME_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public Array Smscount { get; set; }
        public Array Emailcount { get; set; }
        public long totalsms { get; set; }
        public long totalEmail { get; set; }
        public int castid { get; set; }
        public string caste { get; set; }
        public int balance { get; set; }
        public decimal paid { get; set; }
        public decimal receivable { get; set; }
        public decimal concession { get; set; }

        public Array studbal { get; set; }
        public Array fillsection { get; set; }
        public Array castedetails { get; set; }
        public Array fillsectioncount { get; set; }
        public long asmS_Id { get; set; }
        public string section { get; set; }
        public long classid { get; set; }
        public long asmcL_Id { get; set; }
        public Array Fillstudentstrenth { get; set; }
        public Array yearlist { get; set; }
        public Array fillclass { get; set; }
        public Array fillabsent { get; set; }
        public Array fillfee { get; set; }
        public Array coedata { get; set; }
        public Array newadmstd { get; set; }
        public Array fillregstd { get; set; }
        public Array fillnewadmstd { get; set; }
        public Array sectionwisestrenth { get; set; }

        public Array classarray { get; set; }
        public Array sectionarray { get; set; }
        public Array sectionwisestrenthnewadm { get; set; }

        public string sectionname { get; set; }

        public string year { get; set; }



        public string name { get; set; }
        public string admno { get; set; }
        public string regno { get; set; }
        public string gender { get; set; }
        public string mobile { get; set; }
        public string feeclass { get; set; }
        public string Class_Name { get; set; }
        public long stud_count { get; set; }



    }
}
