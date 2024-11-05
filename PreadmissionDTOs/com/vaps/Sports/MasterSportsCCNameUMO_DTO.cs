using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Sports
{
    public class MasterSportsCCNameUMO_DTO:CommonParamDTO
    {
        public long SPCCMSCCUOM_Id { get; set; }
        public long SPCCMSCC_Id { get; set; }
        public long MI_Id { get; set; }
        public long SPCCMUOM_Id { get; set; }
        public bool SPCCMSCCUOM_ActiveFlag { get; set; }
        public int count { get; set; }
        public string returnVal { get; set; }
        public Array sportsCCNameList { get; set; }
        public Array uomList { get; set; }
        public Array sportsCCNameUOMList { get; set; }
        public Array editDetails { get; set; }
        public string sportsCCName { get; set; }
        public string uomName { get; set; }
        public bool retval{ get; set; }

    }
}
