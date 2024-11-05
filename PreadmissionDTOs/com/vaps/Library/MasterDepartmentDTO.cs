using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Library
{
    public class MasterDepartmentDTO:CommonParamDTO
    {
        public long LMD_Id { get; set; }
        public long MI_Id { get; set; }
        public string LMD_DepartmentName { get; set; }
        public bool LMD_ActiveFlg { get; set; }
        public bool returnval { get; set; }
        public bool duplicate { get; set; }
        public Array deptlist { get; set; }
        public string LMD_DepartmentCode { get; set; }

    }
}
