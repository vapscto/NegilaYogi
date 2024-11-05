using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Interfaces
{
   public interface QualificationReportInterface
    {
        MasterEmployeeDTO getalldetails(MasterEmployeeDTO data);
        Task<MasterEmployeeDTO> getQualificationReport(MasterEmployeeDTO data);
    }
}
