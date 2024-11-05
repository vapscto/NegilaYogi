using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class HR_Master_ReferenceDTO : CommonParamDTO
    {
        public long HRMR_Id { get; set; }
        public string HRMR_Name { get; set; }
        public int HRMR_Order { get; set; }
        public string HRMR_Desc { get; set; }
        public bool HRMR_ActiveFlg { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }

}