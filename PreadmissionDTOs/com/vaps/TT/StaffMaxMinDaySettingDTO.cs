using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class StaffMaxMinDaySettingDTO
    {

        public long TTPMMD_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long HRME_Id { get; set; }
        public int TTMP_Id { get; set; }
        public long TTMC_Id { get; set; }
        public long TTPMMD_MaxDay { get; set; }
        public long TTPMMD_MinDay { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool TTPMMD_ActiveFlag { get; set; }
        public string FirstName { get; set; }

        public Array Acdlist { get; set; }
        public Array ctlist { get; set; }
        public Array stafflist { get; set; }
        public Array periodlist { get; set; }
        public Array daylistdetail { get; set; }
        public string academicyr { get; set; }
        public string catgname { get; set; }
        public string stafname { get; set; }
        public string period { get; set; }
        public long maxday { get; set; }
        public long minday { get; set; }
        public Array detailedit { get; set; }
        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }
        public int count { get; set; }
    }
}
