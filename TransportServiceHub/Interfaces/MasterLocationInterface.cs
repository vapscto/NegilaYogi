using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
    public interface MasterLocationInterface
    {
        MasterLocationDTO getdata(int id);
        MasterLocationDTO savedata(MasterLocationDTO data);
        MasterLocationDTO activedeactive(MasterLocationDTO data);
        MasterLocationDTO edit(MasterLocationDTO data);
    }
}
