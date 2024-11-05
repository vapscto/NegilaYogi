using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class MasterRouteScheduleDelegate
    {
        CommonDelegate<MasterRouteScheduleDTO, MasterRouteScheduleDTO> _comm = new CommonDelegate<MasterRouteScheduleDTO, MasterRouteScheduleDTO>();

        public MasterRouteScheduleDTO getdata(int id)
        {
            return _comm.GetDataByIdTransport(id, "MasterRouteScheduleFacade/getdata/");
        }
        public MasterRouteScheduleDTO savedata(MasterRouteScheduleDTO data)
        {
            return _comm.POSTDataTransport(data, "MasterRouteScheduleFacade/savedata/");
        }
        public MasterRouteScheduleDTO edit(MasterRouteScheduleDTO data)
        {
            return _comm.POSTDataTransport(data, "MasterRouteScheduleFacade/edit/");
        }
        public MasterRouteScheduleDTO activedeactive(MasterRouteScheduleDTO data)
        {
            return _comm.POSTDataTransport(data, "MasterRouteScheduleFacade/activedeactive/");
        }
    }
}
