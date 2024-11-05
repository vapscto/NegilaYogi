using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class School_M_ClassDTO : CommonParamDTO
    {
        public long ASMCL_Id { get; set; }
        public long MI_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public int ASMCL_MinAgeYear { get; set; }
        public int ASMCL_MinAgeMonth { get; set; }
        public int ASMCL_MinAgeDays { get; set; }
        public int ASMCL_MaxAgeYear { get; set; }
        public int ASMCL_MaxAgeMonth { get; set; }
        public int ASMCL_MaxAgeDays { get; set; }
        public int ASMCL_Order { get; set; }
        public string ASMCL_ClassCode { get; set; }
        public bool ASMCL_ActiveFlag { get; set; }
        public int ASMCL_MaxCapacity { get; set; }
        public Array School_M_ClassList { get; set; }

        public string returnval { get; set; }
        public string EnteredData { get; set; }
        public string SearchColumn { get; set; }
        public int count { get; set; }
        public string message { get; set; }
        public int ASMCL_PreadmFlag { get; set; }
        public Array getclasslist { get; set; }
    }
}
