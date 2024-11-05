using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
    public interface MasterVehicleInterface
    {
        MasterVehicleDTO getdata(int id);
        MasterVehicleDTO savedata(MasterVehicleDTO data);
        MasterVehicleDTO edit(MasterVehicleDTO data);
        MasterVehicleDTO activedeactive(MasterVehicleDTO data);
        MasterVehicleDTO validaevehicleno(MasterVehicleDTO data);
        MasterVehicleDTO rcreport(MasterVehicleDTO data);
        MasterVehicleDTO validaevhassiseno(MasterVehicleDTO data);
        MasterVehicleDTO validaeengineno(MasterVehicleDTO data);
        MasterVehicleDTO deleteuploadfile(MasterVehicleDTO data);
        MasterVehicleDTO viewuploadflies(MasterVehicleDTO data);
    }
}
