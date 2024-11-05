using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace PortalHub.com.vaps.Student.Interfaces
{
    public interface ClassWorkDownloadInterface
    {
       IVRM_ClassWorkDTO getloaddata(IVRM_ClassWorkDTO data);
        IVRM_ClassWorkDTO getwork(IVRM_ClassWorkDTO sddto);
    }
}
