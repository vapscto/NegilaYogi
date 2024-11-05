using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class HR_Master_PositionDTO : CommonParamDTO
    {
        public long HRMP_Id { get; set; }
        public string HRMP_Position { get; set; }
        public string HRMP_Skills { get; set; }
        public string HRMP_Desc { get; set; }
        public bool HRMP_ActiveFlg { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
        public string retrunMsg { get; set; }
        public Array PositionList { get; set; }
        public Array institutionlist { get; set; }
        public long MI_Id { get; set; }
        public long userid { get; set; }
        public long HRMP_CreatedBy { get; set; }
        public long HRMP_UpdatedBy { get; set; }
    }

}