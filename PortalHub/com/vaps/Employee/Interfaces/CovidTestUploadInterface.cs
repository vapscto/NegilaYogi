using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Interfaces
{
    public interface CovidTestUploadInterface
    {
        CovidTestUploadDTO onloaddata(CovidTestUploadDTO data);
        CovidTestUploadDTO saverecord(CovidTestUploadDTO data);
        CovidTestUploadDTO deactiveY(CovidTestUploadDTO data);
    }
}
