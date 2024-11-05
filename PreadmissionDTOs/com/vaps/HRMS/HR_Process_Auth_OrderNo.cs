using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Process_Auth_OrderNo : CommonParamDTO
    {

        public long HRPAON_Id { get; set; }
        public long HRPA_Id { get; set; }
        public long IVRMUL_Id { get; set; }

        public long HRPAON_SanctionLevelNo { get; set; }
        public bool HRPAON_FinalFlg { get; set; }
        public long LogInUserId { get; set; }
    }
}
