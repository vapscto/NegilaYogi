using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class AdmCollegeStudentGuardianDTO:CommonParamDTO
    {
        public long ACSTG_Id { get; set; }
        public long AMCST_Id { get; set; }
        public string ACSTG_GuardianName { get; set; }
        public string ACSTG_GuardianAddress { get; set; }
        public long ACSTG_GuardianPhoneNo { get; set; }
        public string ACSTG_emailid { get; set; }
        public string ACSTG_GuardianPhoto { get; set; }
        public string ACSTG_GuardianSign { get; set; }
        public string ACSTG_Fingerprint { get; set; }
        public bool ACSTG_GuardianLoginFlag { get; set; }
        public string ACSTG_CoutryCode { get; set; }

    }
}
