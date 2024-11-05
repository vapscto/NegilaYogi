using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class classsectionorderwrapperDTO
    {
        public int miid { get; set; }   
        public int ASMCL_Order { get; set; }
        public List<ClasssectionorderDTO> classorder { get; set; }
        public List<ClasssectionorderDTO> secorder { get; set; }
    }
}
