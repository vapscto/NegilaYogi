using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class StudentGuardianDTO : CommonParamDTO
    {
        public long PASRG_Id { get; set; }
        public long PASR_Id { get; set; }
        public string PASRG_GuardianName { get; set; }
        public string PASRG_GuardianAddress { get; set; }
        public int PASRG_GuardianPhoneNo { get; set; }
        public string PASRG_emailid { get; set; }
        public int PASRG_GuardianLoginFlag { get; set; }
    }
}
