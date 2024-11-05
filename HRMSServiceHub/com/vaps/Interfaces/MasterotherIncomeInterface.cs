using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface MasterotherIncomeInterface
    {
        HR_master_otherIncomeDTO getBasicData(HR_master_otherIncomeDTO dto);
        HR_master_otherIncomeDTO SaveUpdate(HR_master_otherIncomeDTO dto);
        HR_master_otherIncomeDTO editData(int id);

        HR_master_otherIncomeDTO deactivate(HR_master_otherIncomeDTO dto);
    }
}
