using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class FODayNameDTO
    {
        public long FOMD_Id { get; set; }

        public long MI_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public string FOMD_DayName { get; set; }
        public string FOMD_DayCode { get; set; }
        public bool FOMD_ActiveFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
