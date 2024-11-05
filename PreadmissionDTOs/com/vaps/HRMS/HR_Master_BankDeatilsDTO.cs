using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Master_BankDeatilsDTO :CommonParamDTO
    {
        public long HRMBD_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMBD_BankName { get; set; }
        public string HRMBD_BankAccountNo { get; set; }
        public string HRMBD_BankAddress { get; set; }
        public string HRMBD_BranchName { get; set; }
        public string HRMBD_IFSCCode { get; set; }
        public bool HRMBD_ActiveFlag { get; set; }
        public Array bankdetailList { get; set; }
        public string retrunMsg { get; set; }

        public long roleId { get; set; }

    }
}
