using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Interfaces
{
  public interface HWMonthEndReportInterface
    {
        HomeWorkUploadDTO getdata123(HomeWorkUploadDTO data);
        Task<HomeWorkUploadDTO> getreport(HomeWorkUploadDTO data);
        
    }
}
