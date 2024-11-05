using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class HR_Candidate_QualificationsDTO : CommonParamDTO
    {
        public long HRCQUAL_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRCD_Id { get; set; }
        public string HRCQUAL_Course { get; set; }
        public string HRCQUAL_Board { get; set; }
        public long HRCQUAL_PassingYear { get; set; }
        public bool HRCQUAL_ActiveFlag { get; set; }
        public long HRCQUAL_CreatedBy { get; set; }
        public long HRCQUAL_UpdatedBy { get; set; }
    }

}