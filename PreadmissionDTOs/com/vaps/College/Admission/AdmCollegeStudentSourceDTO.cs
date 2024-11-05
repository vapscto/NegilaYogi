using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class AdmCollegeStudentSourceDTO:CommonParamDTO
    {
        public long ACSTS_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ASRS_Id { get; set; }
        public long PAMS_Id { get; set; }
    }
}
