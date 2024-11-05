using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
   public interface VehicalRouteMappingInterface
    {

        VehicalRouteMappingDTO getdata(int id);

        VehicalRouteMappingDTO savedata(VehicalRouteMappingDTO data);

        VehicalRouteMappingDTO editdata(VehicalRouteMappingDTO data);


        VehicalRouteMappingDTO activedeactive(VehicalRouteMappingDTO data);

    }
}
