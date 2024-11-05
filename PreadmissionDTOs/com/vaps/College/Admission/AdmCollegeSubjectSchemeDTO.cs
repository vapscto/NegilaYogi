using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class AdmCollegeSubjectSchemeDTO
    {
        public long ACSS_Id { get; set; }
        public long MI_Id { get; set; }
        public string ACSS_SchmeName { get; set; }
        public bool ACST_ActiveFlg { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }
        public Array sublist { get; set; }
    }
}
