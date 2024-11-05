using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class GovernmentBondDTO:CommonParamDTO
    {

        public long IMGB_Id { get; set; }
        public long MI_Id { get; set; }
        public string IMGB_Name { get; set; }
        public string IMGB_Description { get; set; }
        public Array GovernmentBondname { get; set; }

        public string returnMsg { get; set; }
        public long AMST_Id { get; set; }
        public long AMSTB_Id { get; set; }
        public string AMSTB_BondName { get; set; }
        public int AMSTB_BandNo { get; set; }
        public int count { get; set; }
    }
}
