using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_MasterExam_GroupBDTO : CommonParamDTO
    {
        public long HRMEGB_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMEGB_GroupBExamName { get; set; }
        public bool HRMEGB_ActiveFlg { get; set; }
        public long HRMEGB_CreatedBy { get; set; }
        public long HRMEGB_UpdatedBy { get; set; }
        public long roleId { get; set; }
        public string retrunMsg { get; set; }
        public Array examdetailList { get; set; }
    }

}