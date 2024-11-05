using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class MasterSubjectDTO : CommonParamDTO
    {
        public long PAMS_Id { get; set; }
        public long MI_Id { get; set; }
        public string PAMS_SubjectName { get; set; }
        public string PAMS_SubjectCode { get; set; }
        public decimal PAMS_MaxMarks { get; set; }
        public decimal PAMS_MinMarks { get; set; }
        public string PAMS_SubjectFlag { get; set; }
        public int PAMS_ActiveFlag { get; set; }
        public Array MasterSubjectData { get; set; }
        public String returnval { get; set; }
    }
}
