using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Interfaces
{
    public interface HomeworkUploadInterface
    {
        HomeWorkUploadDTO Getdata_class(HomeWorkUploadDTO dto);
        HomeWorkUploadDTO Getdataview(HomeWorkUploadDTO dto);
        HomeWorkUploadDTO getreport_class(HomeWorkUploadDTO dto);
        HomeWorkUploadDTO getreport_home(HomeWorkUploadDTO dto);
      
        HomeWorkUploadDTO getreport_notice(HomeWorkUploadDTO dto);
        HomeWorkUploadDTO getsection(HomeWorkUploadDTO dto);

        HomeWorkUploadDTO getseenreport(HomeWorkUploadDTO dto);
        HomeWorkUploadDTO Getdataview_seen(HomeWorkUploadDTO dto);
        HomeWorkUploadDTO viewData(HomeWorkUploadDTO data);
    }
}
