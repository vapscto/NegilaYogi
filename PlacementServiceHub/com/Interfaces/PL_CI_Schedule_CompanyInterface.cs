using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlacementServiceHub.com.Interfaces
{
    public interface PL_CI_Schedule_CompanyInterface
    {
        PL_CI_Schedule_CompanyDTO loaddata(PL_CI_Schedule_CompanyDTO data);
  
        PL_CI_Schedule_CompanyDTO savedata(PL_CI_Schedule_CompanyDTO data);
        PL_CI_Schedule_CompanyDTO deactive(PL_CI_Schedule_CompanyDTO data);
        PL_CI_Schedule_CompanyDTO editdetails(PL_CI_Schedule_CompanyDTO data);
    }
}
