using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface masterpointInterface
    {
        HR_Employee_AssesmentpointsDTO getBasicData(HR_Employee_AssesmentpointsDTO dto);
        HR_Employee_AssesmentpointsDTO SaveUpdate(HR_Employee_AssesmentpointsDTO dto);
        HR_Employee_AssesmentpointsDTO editData(int id);

        HR_Employee_AssesmentpointsDTO deactivate(HR_Employee_AssesmentpointsDTO dto);
    }
}
