using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
    public interface MsterSessionInterface
    {
        MsterSessionDTO getdata(int id);
        MsterSessionDTO activedeactive(MsterSessionDTO data);
        MsterSessionDTO savedata(MsterSessionDTO data);
        MsterSessionDTO edit(MsterSessionDTO data);
    }
}
