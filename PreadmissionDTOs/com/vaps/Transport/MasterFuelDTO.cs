using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PreadmissionDTOs.com.vaps.Transport
{
    public class MasterFuelDTO
    {

        public long TRMFT_Id { get; set; }
        public long MI_Id { get; set; }
        public string TRMFT_FuelType { get; set; }
       
        public bool TRMFT_ActiveFlg { get; set; }
        public string message { get; set; }
        public bool retrval { get; set; }
        public Array getmasterfuel { get; set; }
        public Array geteditdatafuel { get; set; }
    }
}
