using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class Fee_Master_ConcessionDTO : CommonParamDTO
    {
        public long FMCC_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMCC_ConcessionName { get; set; }
        public string FMCC_ConcessionFlag { get; set; }
        public string FMCC_ConcessionApplLimit { get; set; }
        public bool FMCC_ActiveFlag { get; set; }
    }
}
