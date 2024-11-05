using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class StateDTO : CommonParamDTO
    {
        public int amsta_id { get; set; }
        public string amsta_name { get; set; }
        //public int amcon_id { get; set; }
        //public string amcon_name { get; set; }
        //public Array countryDrpDown { get; set; }
        public Array stateDrpDown { get; set; }
        public int ccodelength { get; set; }
        
        public string defaultcurrency { get; set; }
        public string IVRMMC_CountryPhCode { get; set; }

        public Array subcastedrp { get; set; }

        public Array subcastedrpf { get; set; }

        public Array subcastedrpm { get; set; }

        public Array prospectusdetails { get; set; }

        public Array routelist { get; set; }

        public Array locationlist { get; set; }

        public Array admissioncatdrp { get; set; }
    }
}
