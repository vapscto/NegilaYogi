using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class StudentYearLosReportDTO
    {
        public long ASYST_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMC_Id { get; set; }
        public long ASMAY_Id { get; set; }


        public Array fillyear { get; set; }

        public Array fillclass { get; set; }

        public Array fillsection { get; set; }

        public StudentTcReportDTO[] TempararyArrayheadList { get; set; }

        public string tcperortemp { get; set; }
        public string tcallorindi { get; set; }


        public Array alldatagridreport { get; set; }

        public int mid { get; set; }
    }
}
