using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class HR_MRF_ListDTO : CommonParamDTO
    {
        public long HRMRFL_Id { get; set; }
        public long HRMP_Id { get; set; }
        public long HRMPT_Id { get; set; }
        public int HRMRFL_No { get; set; }
        public long HRCD_Id { get; set; }
        public string HRMRFL_Status { get; set; }
        public long IVRMMC_Id { get; set; }
        public long IVRMMS_Id { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }

}