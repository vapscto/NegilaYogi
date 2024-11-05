using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TransportServiceHub.Interfaces;
using PreadmissionDTOs.com.vaps.Transport;
using PreadmissionDTOs;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TransportServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class CLGStudentBuspassFormFacade : Controller
    {
        public CLGStudentBuspassFormInterface _ads;

        public CLGStudentBuspassFormFacade(CLGStudentBuspassFormInterface areaz)
        {
            _ads = areaz;
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
        public CLGStudentBuspassFormDTO getloaddata([FromBody]CLGStudentBuspassFormDTO data)
        {
            return _ads.getloaddata(data);
        }

        [Route("getloaddataintruction")]
        public CLGStudentBuspassFormDTO getloaddataintruction([FromBody]CLGStudentBuspassFormDTO data)
        {
            return _ads.getloaddataintruction(data);
        }

        [Route("getstudata")]
        public CLGStudentBuspassFormDTO getstudata([FromBody]CLGStudentBuspassFormDTO sddto)
        {
            return _ads.getstudata(sddto);
        }

        [Route("getbuspassdata")]
        public Task<CLGStudentBuspassFormDTO> getbuspassdata([FromBody]CLGStudentBuspassFormDTO sddto)
        {
            return _ads.getbuspassdata(sddto);
        }
        [Route("academicload")]
        public CLGStudentBuspassFormDTO academicload([FromBody]CLGStudentBuspassFormDTO data)
        {
            return _ads.academicload(data);
        }
        [Route("getroutedata")]
        public CLGStudentBuspassFormDTO getroutedata([FromBody]CLGStudentBuspassFormDTO sddto)
        {
            return _ads.getroutedata(sddto);
        }

        [Route("getlocationdata")]
        public CLGStudentBuspassFormDTO getlocationdata([FromBody]CLGStudentBuspassFormDTO sddto)
        {
            return _ads.getlocationdata(sddto);
        }

        [Route("getlocationdataonly")]
        public CLGStudentBuspassFormDTO getlocationdataonly([FromBody]CLGStudentBuspassFormDTO sddto)
        {
            return _ads.getlocationdataonly(sddto);
        }

        [Route("savedata")]
        public CLGStudentBuspassFormDTO savedata([FromBody]CLGStudentBuspassFormDTO student)
        {
            return _ads.savedata(student);
        }

        //[Route("paynow")]
        //public CLGStudentBuspassFormDTO paynow([FromBody] CLGStudentBuspassFormDTO dt)
        //{
        //    return _ads.paynow(dt);
        //}
        //[Route("paynow1")]
        //public CLGStudentBuspassFormDTO paynow1([FromBody] CLGStudentBuspassFormDTO dt)
        //{
        //    return _ads.paynow1(dt);
        //}
        //[Route("paynow2")]
        //public CLGStudentBuspassFormDTO paynow2([FromBody] CLGStudentBuspassFormDTO dt)
        //{
        //    return _ads.paynow2(dt);
        //}

        //[Route("getpaymentresponse/")]
        //public PaymentDetails getpaymentresponse([FromBody]PaymentDetails response)
        //{
        //    return _ads.payuresponse(response);
        //}

        //[Route("Razorpaypaymentresponse/")]
        //public PaymentDetails Razorpaypaymentresponse([FromBody]PaymentDetails response)
        //{
        //    return _ads.Razorpaypaymentresponse(response);
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
