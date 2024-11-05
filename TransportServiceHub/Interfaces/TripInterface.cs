using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
   public interface TripInterface
    {
        TripDTO getdata(TripDTO obj);
        TripDTO SearchByBookingId(TripDTO data);
        TripDTO duprecpcheck(TripDTO data);
        TripDTO getvahicle(TripDTO data);
        TripDTO save(TripDTO dto);
        TripDTO SearchByTripId(TripDTO dto);
        TripDTO getbillNo(TripDTO dto);
        TripDTO pay(TripDTO data);
        TripDTO GetTripDetails(TripDTO dt);
        TripDTO approveTrip(TripDTO ds);
        TripDTO rejectTrip(TripDTO ds);
        TripDTO viewDetails(TripDTO data);
        TripDTO printrecept(TripDTO data);
        TripDTO printtripsheet(TripDTO data);
        TripDTO deletetrip(TripDTO data);
        TripDTO printbill(TripDTO data);

        
    }
}
