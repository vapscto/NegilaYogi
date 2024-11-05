using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class ClasssectionorderDTO
    {
        public Array classdetails { get; set; }
        public Array sectiondetails { get; set; }
        public List<ClasssectionorderDTO> classorder { get; set; }
        public List<ClasssectionorderDTO> secorder { get; set; }
        public bool retruval { get; set; }
        public int miid { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public int ASMCL_Order { get; set; }
        public int ASMC_Order { get; set;}
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string flagsec { get; set; }
        public string flagclass { get; set; }
    }
}
