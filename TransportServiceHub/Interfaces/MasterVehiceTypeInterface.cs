using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
    public interface MasterVehicleTypeInterface
    {

        MasterVehicleTypeDTO getdata(int id);
        MasterVehicleTypeDTO savedata(MasterVehicleTypeDTO data);
        MasterVehicleTypeDTO geteditdata(MasterVehicleTypeDTO data);
        MasterVehicleTypeDTO activedeactive(MasterVehicleTypeDTO data);
    }
}
