using CommonLibrary;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class MasterRouteDelegate
    {
        CommonDelegate<MasterRouteDTO, MasterRouteDTO> _route = new CommonDelegate<MasterRouteDTO, MasterRouteDTO>();
        public MasterRouteDTO getdata(int id)
        {
            return _route.GetDataByIdTransport(id, "MasterRouteFacade/getdata/");
        }
        public MasterRouteDTO savedata(MasterRouteDTO data)
        {
            return _route.POSTDataTransport(data, "MasterRouteFacade/savedata/");
        }
        public MasterRouteDTO edit(MasterRouteDTO data)
        {
            return _route.POSTDataTransport(data, "MasterRouteFacade/edit/");
        }
        public MasterRouteDTO activedeactive(MasterRouteDTO data)
        {
            return _route.POSTDataTransport(data, "MasterRouteFacade/activedeactive/");
        }
        public MasterRouteDTO getstudentlistre(MasterRouteDTO data)
        {
            return _route.POSTDataTransport(data, "MasterRouteFacade/getstudentlistre/");
        }
        public MasterRouteDTO saveorder(MasterRouteDTO data)
        {
            return _route.POSTDataTransport(data, "MasterRouteFacade/saveorder/");
        }
        public MasterRouteDTO saveroutearea(MasterRouteDTO data)
        {
            return _route.POSTDataTransport(data, "MasterRouteFacade/saveroutearea/");
        }
        public MasterRouteDTO activedeactiveroutearea(MasterRouteDTO data)
        {
            return _route.POSTDataTransport(data, "MasterRouteFacade/activedeactiveroutearea/");
        }
    }
}
