using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class StaffReplacementUnalocatedPeriodDTO
    {


        public bool returnval { get; set; }

        public long MI_Id { get; set; }
        public Array catelist { get; set; }
        public Array academiclist { get; set; }
        public Array periodslst { get; set; }
        public Array datalst { get; set; }
        public Array classbycategory { get; set; }
        public long TTMC_Id { get; set; }
        public string TTMC_CategoryName { get; set; }
        public long ASMAY_Id { get; set; }
        public Array staffDrpDwn { get; set; }
        public string staffNamelst { get; set; }
        public Array gridweeks { get; set; }
        public long HRME_Id { get; set; }
        public decimal TTMD_Id { get; set; }
        public decimal TTMP_Id { get; set; }
        public long ISMS_Id { get; set; }
        public string TTMSUAB_Abbreviation { get; set; }

    }
}
