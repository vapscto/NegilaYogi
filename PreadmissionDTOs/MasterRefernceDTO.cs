using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class MasterRefernceDTO : CommonParamDTO
    {
        public long PAMR_Id { get; set; }

        public string PAMR_ReferenceName { get; set; }
        public string PAMR_ReferenceDesc { get; set; }
        public Array RefernceData { get; set; }

        public string message { get; set; }
        public string returnval_update { get; set; }
        public string returnval_save { get; set; }
        public string returnval { get; set; }
        public string msgsaveupdate { get; set; }

    }
}
