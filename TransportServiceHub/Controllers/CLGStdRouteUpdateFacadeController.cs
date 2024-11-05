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
    public class CLGStdRouteUpdateFacadeController : Controller
    {
        public CLGStdRouteUpdateInterface _ads;
        public CLGStdRouteUpdateFacadeController(CLGStdRouteUpdateInterface detail)
        {
            _ads = detail;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        [Route("getloaddata")]
        public CLGStdRouteUpdateDTO getloaddata([FromBody]CLGStdRouteUpdateDTO data)
        {
            return _ads.getloaddata(data);
        }

        [Route("getloaddataintruction")]
        public CLGStdRouteUpdateDTO getloaddataintruction([FromBody]CLGStdRouteUpdateDTO data)
        {
            return _ads.getloaddataintruction(data);
        }

        [Route("getstudata")]
        public CLGStdRouteUpdateDTO getstudata([FromBody]CLGStdRouteUpdateDTO sddto)
        {
            return _ads.getstudata(sddto);
        }

        [Route("getbuspassdata")]
        public Task<CLGStdRouteUpdateDTO> getbuspassdata([FromBody]CLGStdRouteUpdateDTO sddto)
        {
            return _ads.getbuspassdata(sddto);
        }

        [Route("getbuspassdataupdate")]
        public Task<CLGStdRouteUpdateDTO> getbuspassdataupdate([FromBody]CLGStdRouteUpdateDTO sddto)
        {
            return _ads.getbuspassdataupdate(sddto);
        }

        [Route("getroutedata")]
        public CLGStdRouteUpdateDTO getroutedata([FromBody]CLGStdRouteUpdateDTO sddto)
        {
            return _ads.getroutedata(sddto);
        }

        [Route("getlocationdata")]
        public CLGStdRouteUpdateDTO getlocationdata([FromBody]CLGStdRouteUpdateDTO sddto)
        {
            return _ads.getlocationdata(sddto);
        }

        [Route("getlocationdataonly")]
        public CLGStdRouteUpdateDTO getlocationdataonly([FromBody]CLGStdRouteUpdateDTO sddto)
        {
            return _ads.getlocationdataonly(sddto);
        }

        [Route("savedata")]
        public CLGStdRouteUpdateDTO savedata([FromBody]CLGStdRouteUpdateDTO student)
        {
            return _ads.savedata(student);
        }

        [Route("paynow")]
        public CLGStdRouteUpdateDTO paynow([FromBody] CLGStdRouteUpdateDTO dt)
        {
            return _ads.paynow(dt);
        }

        //[Route("getpaymentresponse/")]
        //public PaymentDetails getpaymentresponse([FromBody]PaymentDetails response)
        //{
        //    return _ads.payuresponse(response);
        //}

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


        [Route("searchfilter")]
        public CLGStdRouteUpdateDTO searchfilter([FromBody]CLGStdRouteUpdateDTO sddto)
        {
            return _ads.searchfilter(sddto);
        }

    }
}
