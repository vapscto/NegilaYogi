using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class MasterReligionDTO : CommonParamDTO
    {
        public long IVRMMR_Id { get; set; }
        public string IVRMMR_Name { get; set; }
        public bool Is_Active { get; set; }
       
        public Array religionList { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
        public string EnteredData { get; set; }
        public string SearchColumn { get; set; }
        public string operation { get; set; }
    }
}
