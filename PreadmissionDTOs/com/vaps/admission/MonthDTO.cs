using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class MonthDTO
    {
        public long IVRM_Month_Id { get; set; }
        public string IVRM_Month_Name { get; set; }
        public bool Is_Active { get; set; }
        public int IVRM_Month_Max_Days { get; set; }
        public decimal ASCH_ClassHeld { get; set; }
    }
}
