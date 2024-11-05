using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
    public interface MasterFuelInterface
    {
        MasterFuelDTO getdata(int id);
        MasterFuelDTO savedata(MasterFuelDTO data);
        MasterFuelDTO geteditdata(MasterFuelDTO data);
        MasterFuelDTO activedeactive(MasterFuelDTO data);

    }
}
