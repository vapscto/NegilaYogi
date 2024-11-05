using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface MasterBankInterface
    {
        HR_Master_BankDeatilsDTO getBasicData(HR_Master_BankDeatilsDTO dto);
        HR_Master_BankDeatilsDTO SaveUpdate(HR_Master_BankDeatilsDTO dto);
        HR_Master_BankDeatilsDTO editData(int id);

        HR_Master_BankDeatilsDTO deactivate(HR_Master_BankDeatilsDTO dto);
    }
}
