using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface HRMasterEmpFullTimeInterface
    {
        NAACHRMasterEmpFullTimeDTO getalldetails(NAACHRMasterEmpFullTimeDTO dto);
        NAACHRMasterEmpFullTimeDTO savedata(NAACHRMasterEmpFullTimeDTO dto);
        NAACHRMasterEmpFullTimeDTO editRecord(NAACHRMasterEmpFullTimeDTO dto);
        NAACHRMasterEmpFullTimeDTO ActiveDeactiveRecord(NAACHRMasterEmpFullTimeDTO dto);
    }
}
