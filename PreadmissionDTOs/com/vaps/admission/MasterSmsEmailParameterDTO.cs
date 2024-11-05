using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class MasterSmsEmailParameterDTO : CommonParamDTO
    {
        public long ISMP_ID { get; set; }
        public string ISMP_NAME { get; set; }
        public string ISMP_Query { get; set; }
        public Array parameterlist { get; set; }
        public Array parms { get; set; }
        public Array editlist { get; set; }
        public bool returnVal { get; set; }
        public int count { get; set;}
        public string message { get; set; }
        public string messageupdate { get; set; }
        public string ISMP_ParameterDesc { get; set; }
        public long ISMHTML_Id { get; set; }
        public long User_id { get; set; }
        public long MI_Id { get; set; }
        public string ISMHTML_HTMLName { get; set; }
        public string ISMHTML_HTMLTemplate { get; set; }
        public bool ISMHTML_ActiveFlg { get; set; }
        public long ISMHTML_CreatedBy { get; set; }
        public long ISMHTML_UpdatedBy { get; set; }
    }
}
