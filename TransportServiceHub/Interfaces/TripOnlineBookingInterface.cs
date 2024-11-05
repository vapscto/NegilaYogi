using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
   public interface TripOnlineBookingInterface
    {
        TripOnlineBookingDTO getdata(TripOnlineBookingDTO dto);
        TripOnlineBookingDTO save(TripOnlineBookingDTO dto);
        TripOnlineBookingDTO edit(int Id);
        TripOnlineBookingDTO deactvate(TripOnlineBookingDTO dto);
        TripOnlineBookingDTO setsessionvalue(TripOnlineBookingDTO dto);
        TripOnlineBookingDTO getHirer(TripOnlineBookingDTO dtos);
        TripOnlineBookingDTO getHirerDetails(TripOnlineBookingDTO dtoss);
    }
}
