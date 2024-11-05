using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class HR_Candidate_ExperienceDTO : CommonParamDTO
    {
        public long HRCEXP_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRCD_Id { get; set; }
        public string HRCEXP_CompanyName { get; set; }
        public string HRCEXP_Designation { get; set; }
        public DateTime? HRCEXP_From { get; set; }
        public DateTime? HRCEXP_To { get; set; }
        public decimal HRCEXP_Salary { get; set; }
        public bool HRCEXP_ActiveFlag { get; set; }
        public long HRCEXP_CreatedBy { get; set; }
        public long HRCEXP_UpdatedBy { get; set; }
    }

}