using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Master_Employee_BankDTO:CommonParamDTO
    {
        public long HRMEB_Id { get; set; }
        public long HRME_Id { get; set; }
        public long HRMBD_Id { get; set; }
        public string HRMEB_AccountNo { get; set; }
        //public long HRMEB_AccountNo { get; set; }
        public string HRMEB_ActiveFlag { get; set; }
    }
}
