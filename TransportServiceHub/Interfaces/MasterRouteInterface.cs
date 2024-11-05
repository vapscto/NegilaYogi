using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
    public interface MasterRouteInterface
    {
        MasterRouteDTO getdata (int id);
        MasterRouteDTO savedata(MasterRouteDTO data);
        MasterRouteDTO edit(MasterRouteDTO data);
        MasterRouteDTO activedeactive(MasterRouteDTO data);
        MasterRouteDTO getstudentlistre(MasterRouteDTO data);
        MasterRouteDTO saveorder(MasterRouteDTO data);
        MasterRouteDTO saveroutearea(MasterRouteDTO data);
        MasterRouteDTO activedeactiveroutearea(MasterRouteDTO data);
    }

}
