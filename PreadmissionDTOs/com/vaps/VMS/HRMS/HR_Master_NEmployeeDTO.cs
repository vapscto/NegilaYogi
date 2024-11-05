using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class HR_Master_NEmployeeDTO : CommonParamDTO
    {
        public long HRMNE_Id { get; set; }
        public string HRMNE_Name { get; set; }
        public int HRMNE_Order { get; set; }
        public bool HRMNE_ActiveFlg { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }

}