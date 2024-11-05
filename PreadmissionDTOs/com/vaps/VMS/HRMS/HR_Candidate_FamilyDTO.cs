using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class HR_Candidate_FamilyDTO : CommonParamDTO
    {
        public long HRCFAM_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRCD_Id { get; set; }
        public string HRCFAM_Name { get; set; }
        public string HRCFAM_Relationship { get; set; }
        public string HRCFAM_Occupation { get; set; }
        public string HRCFAM_CompanyName { get; set; }
        public long HRCFAM_Age { get; set; }
        public bool HRCFAM_ActiveFlag { get; set; }
        public long HRCFAM_CreatedBy { get; set; }
        public long HRCFAM_UpdatedBy { get; set; }
    }

}