using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class HR_Master_JobsDTO : CommonParamDTO
    {
        public long HRMJ_Id { get; set; }
        public string HRMJ_JobCode { get; set; }
        public string HRMJ_JobTiTle { get; set; }
        public long HRMLO_Id { get; set; }
        public string HRMJ_Posted { get; set; }
        public long HRC_Id { get; set; }
        public bool HRMJ_ActiveFlg { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }

        public long MI_Id { get; set; }
        public string retrunMsg { get; set; }
        public string HRMLO_LocationName { get; set; }
        public Array JobList { get; set; }
        public Array locationlist { get; set; }
    }

}