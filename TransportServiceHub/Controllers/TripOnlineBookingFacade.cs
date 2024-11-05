using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Transport;
using TransportServiceHub.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TransportServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class TripOnlineBookingFacade : Controller
    {
        TripOnlineBookingInterface _interface;
        public TripOnlineBookingFacade(TripOnlineBookingInterface intrfce)
        {
            _interface = intrfce;
        }
        [Route("getdata")]
        public TripOnlineBookingDTO getdata([FromBody]TripOnlineBookingDTO dto)
        {
            return _interface.getdata(dto);
        }
        [Route("setsessionvalue")]
        public TripOnlineBookingDTO setsessionvalue([FromBody]TripOnlineBookingDTO data)
        {
            return _interface.setsessionvalue(data);
        }
        [Route("save")]
        public TripOnlineBookingDTO save([FromBody]TripOnlineBookingDTO data)
        {
            return _interface.save(data);
        }
        [Route("getHirer")]
        public TripOnlineBookingDTO getHirer([FromBody]TripOnlineBookingDTO data1)
        {
            return _interface.getHirer(data1);
        }
        [Route("getHirerDetails")]
        public TripOnlineBookingDTO getHirerDetails([FromBody]TripOnlineBookingDTO da)
        {
            return _interface.getHirerDetails(da);
        }
        [Route("edit/{id:int}")]
        public TripOnlineBookingDTO edit(int id)
        {
            return _interface.edit(id);
        }
        [Route("deactivate")]
        public TripOnlineBookingDTO deactivate([FromBody] TripOnlineBookingDTO dto)
        {
            return _interface.deactvate(dto);
        }


    }
}
