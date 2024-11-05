using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
   public  interface MasterAreaInterface
    {
        MasterAreaDTO getdata(int id);
        MasterAreaDTO savedata(MasterAreaDTO data);
        MasterAreaDTO geteditdata(MasterAreaDTO data);
        MasterAreaDTO activedeactive(MasterAreaDTO data);

    }
}
