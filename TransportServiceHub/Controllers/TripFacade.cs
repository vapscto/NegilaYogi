using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TransportServiceHub.Interfaces;
using PreadmissionDTOs.com.vaps.Transport;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TransportServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class TripFacade : Controller
    {
        TripInterface _interface;
        public TripFacade(TripInterface interfaces)
        {
            _interface = interfaces;
        }
        [Route("getdata")]
        public TripDTO getdata([FromBody] TripDTO data)
        {
            return _interface.getdata(data);
        }
        [Route("Search")]
        public TripDTO Search([FromBody] TripDTO data)
        {
            return _interface.SearchByBookingId(data);
        }
        [Route("duprecpcheck")]
        public TripDTO duprecpcheck([FromBody] TripDTO data)
        {
            return _interface.duprecpcheck(data);
        }
        [Route("getvahicle")]
        public TripDTO getvahicle([FromBody] TripDTO data)
        {
            return _interface.getvahicle(data);
        }
        [Route("save")]
        public TripDTO save([FromBody] TripDTO data)
        {
            return _interface.save(data);
        }
        [Route("SearchByTripId")]
        public TripDTO SearchByTripId([FromBody] TripDTO data)
        {
            return _interface.SearchByTripId(data);
        }
        [Route("getbillNo")]
        public TripDTO getbillNo([FromBody] TripDTO dto)
        {
            return _interface.getbillNo(dto);
        }
        [Route("pay")]
        public TripDTO pay([FromBody]TripDTO data)
        {
            return _interface.pay(data);
        }
        [Route("GetTripDetails")]
        public TripDTO GetTripDetails([FromBody] TripDTO ds)
        {
            return _interface.GetTripDetails(ds);
        }
        [Route("approve")]
        public TripDTO approve([FromBody] TripDTO ds)
        {
            return _interface.approveTrip(ds);
        }
        [Route("reject")]
        public TripDTO reject([FromBody] TripDTO ds)
        {
            return _interface.rejectTrip(ds);
        }
        [Route("viewDetails")]
        public TripDTO viewDetails([FromBody] TripDTO ds)
        {
            return _interface.viewDetails(ds);
        }
        [Route("printrecept")]
        public TripDTO printrecept([FromBody] TripDTO ds)
        {
            return _interface.printrecept(ds);
        }
        [Route("printbill")]
        public TripDTO printbill([FromBody] TripDTO ds)
        {
            return _interface.printbill(ds);
        }

        

        [Route("printtripsheet")]
        public TripDTO printtripsheet([FromBody] TripDTO ds)
        {
            return _interface.printtripsheet(ds);
        }

        [Route("deletetrip")]
        public TripDTO deletetrip([FromBody] TripDTO ds)
        {
            return _interface.deletetrip(ds);
        }


        
    }
}
