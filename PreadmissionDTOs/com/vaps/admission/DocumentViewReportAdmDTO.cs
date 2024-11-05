using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class DocumentViewReportAdmDTO
    {
        public long MI_Id { get; set; }
        public Array year { get; set; }
        public Array classname{ get; set; }
        public Array section { get; set; }
        public Array getreport { get; set; }
        public Array document { get; set; }
        public Array studentdetails { get; set; }
        public long AMST_Id { get; set; }
        public string studentname { get; set; }
        public long asmaY_Id { get; set; }
        public long asmcL_Id { get; set; }
        public long asmS_Id { get; set; }
        public string TC_allorind { get; set; }
        public string casteorcategory { get; set; }
        public Array studentlistreport { get; set; }
        public string STDORDOC { get; set; }
        public string SUBORNOT { get; set; }
        public long AMSMD_Id { get; set; }
    }
}
