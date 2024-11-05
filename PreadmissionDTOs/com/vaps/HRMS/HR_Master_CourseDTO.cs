using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Master_CourseDTO :CommonParamDTO
    {
        public long HRMC_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMC_QulaificationName { get; set; }
        public string HRMC_QualificationDesc { get; set; }
        public int HRMC_Order { get; set; }
        public bool HRMC_ActiveFlag { get; set; }
        public bool HRMC_DefaultQualFag { get; set; }
        public bool HRMC_SpecialisationFlag { get; set; }
        public Array courseList { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }
        public HR_Master_CourseDTO[] CourseDTO { get; set; }
    }
}
