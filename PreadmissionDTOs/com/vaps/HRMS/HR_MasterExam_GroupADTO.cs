using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_MasterExam_GroupADTO : CommonParamDTO
    {
        public long HRMEGA_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMEGA_GroupAExamName { get; set; }
        public bool HRMEGA_ActiveFlg { get; set; }
        public long HRMEGA_CreatedBy { get; set; }
        public long HRMEGA_UpdatedBy { get; set; }
        public long roleId { get; set; }
        public string retrunMsg { get; set; }
        public Array examdetailList { get; set; }
    }

}