using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
    public interface VehicalDriverMappingInterface
    {

        VehicalDriverMappingDTO getdata(int id);

        VehicalDriverMappingDTO savedata(VehicalDriverMappingDTO data);

        VehicalDriverMappingDTO activedeactive(VehicalDriverMappingDTO data);

        VehicalDriverMappingDTO editdata(VehicalDriverMappingDTO data);
    }
}
