using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class studentbirthdayreportDTO
    {

        public string AMST_FirstName { get; set; }
        public string AMST_AdmNo { get; set; }
        public long MI_ID { get; set; }
        public long ASMAY_Id { get; set; }


        public DateTime? amst_dob { get; set; }

        public string ASMCL_ClassName { get; set; }

        public string ASMC_SectionName { get; set; }

        public Array studentDetails { get; set; }


        public string flags { get; set; }
        public string flag { get; set; }
        public string flagl { get; set; }
        public string day { get; set; }
        public string months { get; set; }

        public string days1 { get; set; }
        public string days2 { get; set; }

        public string all1 { get; set; }
        public Array accyear { get; set; }
        public int count { get; set; }
        public DateTime? Fromdate { get; set; }
        public DateTime? Todate { get; set; }
        public Array schooldetails { get; set; }
        public Array schooltabledetails { get; set; }
        public Array classlist { get; set; }
        public classlsttwo[] classlsttwo { get; set; }
        public long ASMCL_Id { get; set; }
    }
    public class classlsttwo
    {
        public long ASMCL_Id { get; set; }
      
    }
}
