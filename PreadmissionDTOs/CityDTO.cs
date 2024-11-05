using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class CityDTO : CommonParamDTO
    {
        public long IVRMMCT_Id { get; set; }
        public string IVRMMCT_Name { get; set; }

        public long IVRMMS_Id { get; set; }
        public long IVRMMC_Id { get; set; }

        public Array cityDrpDown { get; set; }

        //public int CMC_Radius { get; set; }
        //public string CMC_Colour { get; set; }
        //public string CMC_Flag { get; set; }
        //public int CMC_level { get; set; }
        //public long CMC_Parent_Id { get; set; }
        //public int CMC_X { get; set; }
        //public int CMC_Y { get; set; }
        //public string CMC_Alias_Name { get; set; }
    }
}
