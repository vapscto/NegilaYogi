using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class RouteSessionScheduleDelegate
    {
        CommonDelegate<RouteSessionScheduleDTO, RouteSessionScheduleDTO> _comm = new CommonDelegate<RouteSessionScheduleDTO, RouteSessionScheduleDTO>();

        public RouteSessionScheduleDTO getdata(int id)
        {
            return _comm.GetDataByIdTransport(id, "RouteSessionScheduleFacade/getdata/");
        }
        public RouteSessionScheduleDTO savedata(RouteSessionScheduleDTO data)
        {
            return _comm.POSTDataTransport(data, "RouteSessionScheduleFacade/savedata/");
        }
        public RouteSessionScheduleDTO routechange(RouteSessionScheduleDTO data)
        {
            return _comm.POSTDataTransport(data, "RouteSessionScheduleFacade/routechange/");
        }
        public RouteSessionScheduleDTO edit(RouteSessionScheduleDTO data)
        {
            return _comm.POSTDataTransport(data, "RouteSessionScheduleFacade/edit/");
        }
        public RouteSessionScheduleDTO showlocationGrid(RouteSessionScheduleDTO data)
        {
            return _comm.POSTDataTransport(data, "RouteSessionScheduleFacade/showlocationGrid/");
        }
        public RouteSessionScheduleDTO activedeactive(RouteSessionScheduleDTO data)
        {
            return _comm.POSTDataTransport(data, "RouteSessionScheduleFacade/activedeactive/");
        }
    }
}
