using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class HR_Master_PostionTypeDTO : CommonParamDTO
    {
        public long HRMPT_Id { get; set; }
        public string HRMPT_Name { get; set; }
        public string HRMPT_Desc { get; set; }
        public bool HRMPT_ActiveFlg { get; set; }
        public long HRMPT_CreatedBy { get; set; }
        public long HRMPT_UpdatedBy { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
        public string retrunMsg { get; set; }
        public Array PositionTypeList { get; set; }
        public Array institutionlist { get; set; }
        public long MI_Id { get; set; }
        public long userid { get; set; }
    }

}