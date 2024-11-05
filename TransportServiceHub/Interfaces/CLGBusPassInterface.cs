using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
   public  interface CLGBusPassInterface
    {
        CLGBusPassDTO getdata(int id);
        CLGBusPassDTO searchdetails(CLGBusPassDTO data);
        Task<CLGBusPassDTO> showmodaldetails(CLGBusPassDTO data);
        CLGBusPassDTO savelist(CLGBusPassDTO data);

    }
}
