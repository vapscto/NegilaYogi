using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class StudentSmartCardLogReportDTO
    {
        public Array studentlist { get; set; }

        public Array alldatagridreport { get; set; }

        public string regornamedetails { get; set; }

        public long asmay_id { get; set; }

        public long MI_ID { get; set; }

        public long Amst_Id { get; set; }

        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }

        public string AMST_RegistrationNo { get; set; }
        public string AMST_AdmNo { get; set; }
        public long AMAY_RollNo { get; set; }


        public string allorindiv { get; set; }
        public string mallorindi { get; set; }
        public string regorname { get; set; }

        public DateTime? dailydate { get; set; }
        public string dailybtedates { get; set; }
        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }
        public string ASMODULE { get; set; }

        public StudentSmartCardLogReportDTO[] TempararyArrayheadList { get; set; }


    }
}
