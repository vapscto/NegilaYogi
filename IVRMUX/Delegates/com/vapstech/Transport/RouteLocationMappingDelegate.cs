using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class RouteLocationMappingDelegate
    {
        CommonDelegate<RouteLocationMappingDTO, RouteLocationMappingDTO> _comm = new CommonDelegate<RouteLocationMappingDTO, RouteLocationMappingDTO>();
        public RouteLocationMappingDTO getdata(int id)
        {
            return _comm.GetDataByIdTransport(id, "RouteLocationMappingFacade/getdata/");
        }
        public RouteLocationMappingDTO savedata(RouteLocationMappingDTO data)
        {
            return _comm.POSTDataTransport(data, "RouteLocationMappingFacade/savedata/");
        }
        public RouteLocationMappingDTO getlocations(RouteLocationMappingDTO data)
        {
            return _comm.POSTDataTransport(data, "RouteLocationMappingFacade/getlocations/");
        }
        public RouteLocationMappingDTO getlocationsarea(RouteLocationMappingDTO data)
        {
            return _comm.POSTDataTransport(data, "RouteLocationMappingFacade/getlocationsarea/");
        }
        
        public RouteLocationMappingDTO activedeactive(RouteLocationMappingDTO data)
        {
            return _comm.POSTDataTransport(data, "RouteLocationMappingFacade/activedeactive/");
        }
        public RouteLocationMappingDTO getOrder(RouteLocationMappingDTO data)
        {
            return _comm.POSTDataTransport(data, "RouteLocationMappingFacade/getOrder/");
        }
        

    }
}
