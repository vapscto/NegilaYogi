using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
    public interface MasterRouteScheduleInterface
    {
        MasterRouteScheduleDTO getdata(int id);
        MasterRouteScheduleDTO savedata(MasterRouteScheduleDTO data);
        MasterRouteScheduleDTO edit(MasterRouteScheduleDTO data);
        MasterRouteScheduleDTO activedeactive(MasterRouteScheduleDTO data);

    }
}
