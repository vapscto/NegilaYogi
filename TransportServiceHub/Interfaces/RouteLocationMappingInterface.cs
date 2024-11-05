using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
    public interface RouteLocationMappingInterface
    {
        RouteLocationMappingDTO getdata(int id);
        RouteLocationMappingDTO savedata(RouteLocationMappingDTO data);
        RouteLocationMappingDTO getlocations(RouteLocationMappingDTO data);
        RouteLocationMappingDTO activedeactive(RouteLocationMappingDTO data);
        RouteLocationMappingDTO getOrder(RouteLocationMappingDTO data);
        RouteLocationMappingDTO getlocationsarea(RouteLocationMappingDTO data);
        

    }
}
