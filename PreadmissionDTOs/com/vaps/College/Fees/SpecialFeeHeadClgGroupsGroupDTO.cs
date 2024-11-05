using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Fees
{
    public class SpecialFeeHeadClgGroupsGroupDTO
    {
        public long FMSFHFH_Id { get; set; }
        public long FMSFH_Id { get; set; }
        public long FMH_Id { get; set; }

        public Array FeeSpecialgroupgrping { set; get; }
        public string Specialrpname { get; set; }

        public string returnduplicatestatus { get; set; }

        public bool returnval { get; set; }

    }
}
