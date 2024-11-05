using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using PreadmissionDTOs.com.vaps.Portals.Principal;


namespace PortalHub.com.vaps.Principal.Interfaces
{
    public interface CareerReportInterface
    {

        CareerReportDTO getalldetails(CareerReportDTO data);
        IVRM_Homework_DTO get_home_classwork(IVRM_Homework_DTO data);
      

    }
}
