using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class castecategoryDTO :CommonParamDTO
    {
        public long IMCC_Id { get; set; }
        public string IMCC_CategoryName { get; set; }
        public string IMCC_CategoryDesc { get; set; }
        public Array castecategoryname { get; set; }
        public bool returnVal { get; set; }
        public int count { get; set;}
        public string message { get; set; }
        public string messageupdate { get; set; }
        public string IMCC_CategoryCode { get; set; }
    }
}
