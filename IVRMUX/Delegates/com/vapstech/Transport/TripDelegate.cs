using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class TripDelegate
    {
        CommonDelegate<TripDTO, TripDTO> _com = new CommonDelegate<TripDTO, TripDTO>();
        public TripDTO getdata(TripDTO dt)
        {
            return _com.POSTDataTransport(dt, "TripFacade/getdata/");
        }
        public TripDTO search(TripDTO data)
        {
            return _com.POSTDataTransport(data, "TripFacade/Search/");
        }
        public TripDTO duprecpcheck(TripDTO data)
        {
            return _com.POSTDataTransport(data, "TripFacade/duprecpcheck/");
        }
        public TripDTO getvahicle(TripDTO data)
        {
            return _com.POSTDataTransport(data, "TripFacade/getvahicle/");
        }
        public TripDTO savedata(TripDTO data)
        {
            return _com.POSTDataTransport(data, "TripFacade/save/");
        }
        public TripDTO SearchByTripId(TripDTO data)
        {
            return _com.POSTDataTransport(data, "TripFacade/SearchByTripId/");
        }
        public TripDTO getbillNo(TripDTO data)
        {
            return _com.POSTDataTransport(data, "TripFacade/getbillNo/");
        }
        public TripDTO pay(TripDTO data)
        {
            return _com.POSTDataTransport(data, "TripFacade/pay");
        }
        public TripDTO GetTripDetails(TripDTO dto)
        {
            return _com.POSTDataTransport(dto, "TripFacade/GetTripDetails");
        }
        public TripDTO approve(TripDTO dto)
        {
            return _com.POSTDataTransport(dto, "TripFacade/approve");
        }
        public TripDTO reject(TripDTO dto)
        {
            return _com.POSTDataTransport(dto, "TripFacade/reject");
        }
        public TripDTO viewDetails(TripDTO dto)
        {
            return _com.POSTDataTransport(dto, "TripFacade/viewDetails");
        }
        public TripDTO printrecept(TripDTO dto)
        {
            return _com.POSTDataTransport(dto, "TripFacade/printrecept");
        }

        public TripDTO printbill(TripDTO dto)
        {
            return _com.POSTDataTransport(dto, "TripFacade/printbill");
        }

        
        public TripDTO printtripsheet(TripDTO dto)
        {
            return _com.POSTDataTransport(dto, "TripFacade/printtripsheet");
        }
        public TripDTO deletetrip(TripDTO dto)
        {
            return _com.POSTDataTransport(dto, "TripFacade/deletetrip");
        }
        
    }
}
