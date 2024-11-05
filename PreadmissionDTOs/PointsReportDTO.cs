using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class PointsReportDTO
    {
        public int mid { get; set; }
        public int yearid { get; set; }
        public Array yeardropDown { get; set; }

        public long asmayid { get; set; }
        public DateTime? from_date { get; set; }
        public DateTime? to_date { get; set; }
        public Array yearfromTo { get; set; }
        public Array studentDetails { get; set; }

        public Array siblinglist { get; set; }

        public string type { get; set; }
        public long PASAP_ID { get; set; }
        public long PASR_Id { get; set; }
        public int PASAP_AGE { get; set; }
        public int PASAP_INCOME { get; set; }
        public int PASAP_CASTE { get; set; }
        public int PASAP_ADRESS { get; set; }
        public int PASAP_QA { get; set; }
        public int PASAP_TOTAL { get; set; }
        public long mi_id { get; set; }      
        public Array fillclass { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public long Caste_Id { get; set; }
        public string PASR_EmailID { get; set; }
        public string PASR_FirstName { get; set; }
        public string PASR_MiddleName { get; set; }
        public string PASR_LastName { get; set; }
        public int PASR_Age { get; set; }
        public long PASRAPS_ID { get; set; }
    }
}
