using PreadmissionDTOs.com.vaps.Portals.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Principal.Interfaces
{
    public interface TimeTableInterface
    {
        TimeTableDTO getdata(TimeTableDTO obj);

        TimeTableDTO getdaily_data(TimeTableDTO data);
    }
}
