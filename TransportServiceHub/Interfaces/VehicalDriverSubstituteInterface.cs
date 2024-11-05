using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
   public interface VehicalDriverSubstituteInterface
    {
        VehicalDriverSubstituteDTO getdata(int id);
        VehicalDriverSubstituteDTO get_driver_data(VehicalDriverSubstituteDTO data);

        VehicalDriverSubstituteDTO savedata(VehicalDriverSubstituteDTO data);

       
        VehicalDriverSubstituteDTO editdata(VehicalDriverSubstituteDTO data);
    }
}
