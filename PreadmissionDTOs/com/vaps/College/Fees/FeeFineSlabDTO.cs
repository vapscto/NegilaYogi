using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class MasterFeeFineSlabClg_DTO:CommonParamDTO
    {
        public long FMFS_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMFS_FineType { get; set; }
        public long FMFS_FromDay { get; set; }
        public long FMFS_ToDay { get; set; }
        public string FMFS_ECSFlag { get; set; }
        public bool FMFS_ActiveFlag { get; set; }
        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }
        public Array GroupFineSlab { get; set; }
        public string message { get; set; }
    }
}
