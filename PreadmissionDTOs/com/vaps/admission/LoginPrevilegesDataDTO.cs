using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class LoginPrevilegesDataDTO
    {
        public long ASALU_Id { get; set; }
        public long ASALUC_Id { get; set; }
        public long ASALUCS_Id { get; set; }
        public string UserName { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string PAMS_SubjectName { get; set; }
        public string ASMAY_Year { get; set; }
    }
}
