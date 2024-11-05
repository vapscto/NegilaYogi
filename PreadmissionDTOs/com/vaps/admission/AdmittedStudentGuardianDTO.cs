using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class AdmittedStudentGuardianDTO:CommonParamDTO
    {
        public long AMSTG_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public string AMSTG_GuardianName { get; set; }
        public string AMSTG_GuardianAddress { get; set; }
        public string AMSTG_GuardianPhoneNo { get; set; }
        public string AMSTG_emailid { get; set; }
        public string AMSTG_GuardianPhoto { get; set; }
        public string AMSTG_GuardianSign { get; set; }
        public string AMSTG_Fingerprint { get; set; }
        public string AMSTG_GuardianLoginFlag { get; set; }

    }
}
