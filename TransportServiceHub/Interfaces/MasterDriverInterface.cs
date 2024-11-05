using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
    public interface MasterDriverInterface
    {
        MasterDriverDTO getdata(int id);
        MasterDriverDTO checkdrivercode(MasterDriverDTO data);
        MasterDriverDTO checkdriverdl(MasterDriverDTO data);
        MasterDriverDTO checkdriverbno(MasterDriverDTO data);
        MasterDriverDTO savedata(MasterDriverDTO data);
        MasterDriverDTO editdata(MasterDriverDTO data);
        MasterDriverDTO activedeactive(MasterDriverDTO data);
        
    }
}
