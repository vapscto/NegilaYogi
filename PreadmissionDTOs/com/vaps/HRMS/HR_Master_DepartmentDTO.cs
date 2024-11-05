using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Master_DepartmentDTO :CommonParamDTO
    {
        public long HRMD_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public Int32? HRMD_Order { get; set; }
        public bool HRMD_ActiveFlag { get; set; }
        public Array departmentList { get; set; }
        public string retrunMsg { get; set; }
        public int? HRMDC_ID { get; set; }

        public long roleId { get; set; }
        public decimal? HRMD_InternalTrainingMinimumHrs { get; set; }

        public HR_Master_DepartmentDTO[] DeraptmentDTO { get; set; }

    }
}
