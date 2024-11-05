using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface masterparameterInterface
    {
        HR_Employee_AssesmentparameterDTO getBasicData(HR_Employee_AssesmentparameterDTO dto);
        HR_Employee_AssesmentparameterDTO SaveUpdate(HR_Employee_AssesmentparameterDTO dto);
        HR_Employee_AssesmentparameterDTO editData(int id);

        HR_Employee_AssesmentparameterDTO deactivate(HR_Employee_AssesmentparameterDTO dto);
    }
}
