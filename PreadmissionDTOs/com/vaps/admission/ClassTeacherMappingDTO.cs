using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class ClassTeacherMappingDTO
    {
        public long MI_Id { get; set; }
        public Array accyear { get; set; }
        public Array accclass { get; set; }
        public Array accsection { get; set; }
        public Array empdetails { get; set; }
        public Array getsavedetails { get; set; }
        public Array geteditdetails { get; set; }
        public string hrmE_EmployeeFirstName { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool retruval { get; set; }
        public string message { get; set; }
        public long IMCT_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMS_SectionName { get; set; }
        public Array onchangestaff { get; set; }
        public string classname { get; set; }
        public string secname { get; set; }
        public long staffpk1 { get; set; }
        public long staffpk2 { get; set; }
        public long HRME_Id1 { get; set; }
        public long HRME_Id2 { get; set; }
        public bool imct_activeflag { get; set; }
        public string employeecode { get; set; }

    }
}
