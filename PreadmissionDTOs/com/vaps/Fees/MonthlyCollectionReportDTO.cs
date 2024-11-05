using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class MonthlyCollectionReportDTO
    {
        public Array studentlist { get; set; }

        public string regornamedetails { get; set; }

        public long AMST_Id { get; set; }

        public long mid { get; set; }
        public long ASMAY_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }

        public string AMST_RegistrationNo { get; set; }
        public string AMST_AdmNo { get; set; }

        public Array fillfeegroup { get; set; }

        public DateTime?  Fromdate { get; set; }
        public DateTime?  Todate { get; set; }
        public string allorindivflag { get; set; }
        public Array alldatagridreportheads { get; set; }

        public Array alldatagridreport { get; set; }
        public string idamstid { get; set; }

       // public Array Tempgroupid { get; set; }
        public MonthlyCollectionReportDTO[] Tempgroupid { get; set; }

        public string columnID { get; set; }
        public string flagleft { get; set; }

        public long userid { get; set; }
        public string reporttype { get; set; }
        public Array fillmastergroup { get; set; }
        public Array customlist { get; set; }
        public Array grouplist { get; set; }

        public string customflag { get; set; }

        public string groupflag { get; set; }
        public string termflag { get; set; }
        public long[] FMGG_Ids { get; set; }
        public long[] FMG_Ids { get; set; }
        public long[] FMT_Ids { get; set; }
        public string term_group { get; set; }
        public string studenttype { get; set; }
        public long chequedate { get; set; }
    }
}
