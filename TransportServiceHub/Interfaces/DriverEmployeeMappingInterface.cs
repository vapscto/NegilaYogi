using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Transport;

namespace TransportServiceHub.Interfaces
{
    public  interface DriverEmployeeMappingInterface
    {
     
        DriverEmployeeMappingDTO getdata(int id);

        DriverEmployeeMappingDTO savedata(DriverEmployeeMappingDTO data);

        DriverEmployeeMappingDTO edit(DriverEmployeeMappingDTO data);
        DriverEmployeeMappingDTO deletedata(DriverEmployeeMappingDTO data);

    }
}
