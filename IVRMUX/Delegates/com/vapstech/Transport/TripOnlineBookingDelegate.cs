using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class TripOnlineBookingDelegate
    {
        CommonDelegate<TripOnlineBookingDTO, TripOnlineBookingDTO> _com = new CommonDelegate<TripOnlineBookingDTO, TripOnlineBookingDTO>();

       
        public TripOnlineBookingDTO load(TripOnlineBookingDTO data)
        {
            return _com.POSTDataTransport(data, "TripOnlineBookingFacade/getdata/");
        }
        public TripOnlineBookingDTO setsessionvalue(TripOnlineBookingDTO obj)
        {
            return _com.POSTDataTransport(obj, "TripOnlineBookingFacade/setsessionvalue/");
        }
        public TripOnlineBookingDTO save(TripOnlineBookingDTO data)
        {
            return _com.POSTDataTransport(data, "TripOnlineBookingFacade/save/");
        }
        public TripOnlineBookingDTO edit(int id)
        {
            return _com.GetDataByIdTransport(id, "TripOnlineBookingFacade/edit/");
        }
        public TripOnlineBookingDTO deactivate(TripOnlineBookingDTO dto)
        {
            return _com.POSTDataTransport(dto, "TripOnlineBookingFacade/deactivate/");
        }
        public TripOnlineBookingDTO getHirer(TripOnlineBookingDTO obj)
        {
            return _com.POSTDataTransport(obj, "TripOnlineBookingFacade/getHirer");
        }
        public TripOnlineBookingDTO getHirerDetails(TripOnlineBookingDTO obj)
        {
            return _com.POSTDataTransport(obj, "TripOnlineBookingFacade/getHirerDetails");
        }
    }
}
