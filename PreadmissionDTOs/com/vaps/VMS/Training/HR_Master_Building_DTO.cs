using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace PreadmissionDTOs.com.vaps.VMS.Training
{
    public class HR_Master_Building_DTO : CommonParamDTO
    {
        public long HRMB_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMB_BuildingName { get; set; }
        public string HRMB_Desc { get; set; }
        public bool HRMB_ActiveFlag { get; set; }
        public long HRMB_CreatedBy { get; set; }
        public long HRMB_UpdatedBy { get; set; }
        public long userId { get; set; }
        public Array building_list { get; set; }
        public Array flrlist { get; set; }
        public bool returnval { get; set; }
        public string returnvales { get; set; }
    }
}
