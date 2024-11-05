using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
    public interface RouteSessionScheduleInterface
    {
        RouteSessionScheduleDTO getdata(int id);
        RouteSessionScheduleDTO savedata(RouteSessionScheduleDTO data);
        RouteSessionScheduleDTO routechange(RouteSessionScheduleDTO data);
        RouteSessionScheduleDTO edit(RouteSessionScheduleDTO data);
        RouteSessionScheduleDTO showlocationGrid(RouteSessionScheduleDTO data);
        RouteSessionScheduleDTO activedeactive(RouteSessionScheduleDTO data);

    }
}
