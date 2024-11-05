using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PreadmissionDTOs
{
    public class EnqDTO : CommonParamDTO
    {
        public int IVRMMS_Id { get; set; }

        public string IVRMMS_Name { get; set; }
        public int IVRMMC_Id { get; set; }

        public string IVRMMC_CountryName { get; set; }
        public long IVRMMCT_Id { get; set; }
        public string IVRMMCT_Name { get; set; }


        public Array countryDrpDown { get; set; }
        public Array stateDrpDown { get; set; }
        public Array cityDrpDown { get; set; }
       // public Array countryDrpDown { get; set; }
        public Array courseDrpDown { get; set; }
        //   public Array stateDrpDown { get; set; }
        // public Array cityDrpDown { get; set; }

        //sripad added
        public Array yearDrpDwn { get; set; }
        public Array classDrpDwn { get; set; }
        public Array categoryDrpDwn { get; set; }

        public Array organisationname { get; set; }
        public Array enqdata { get; set; }
        public string GeneratedNumber { get; set; }


    }
}
