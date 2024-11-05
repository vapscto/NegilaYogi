using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Student.Interfaces;
using PreadmissionDTOs;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Student.Controllers
{
    [Route("api/[controller]")]
    public class StudentBuspassFormFacadeController : Controller
    {
        public StudentBuspassFormInterface _ads;

        public StudentBuspassFormFacadeController(StudentBuspassFormInterface adstu)
        {
            _ads = adstu;
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
        public StudentBuspassFormDTO getloaddata([FromBody]StudentBuspassFormDTO data)
        {
            return _ads.getloaddata(data);
        }

        [Route("getloaddataintruction")]
        public StudentBuspassFormDTO getloaddataintruction([FromBody]StudentBuspassFormDTO data)
        {
            return _ads.getloaddataintruction(data); 
        }

        [Route("getstudata")]
        public StudentBuspassFormDTO getstudata([FromBody]StudentBuspassFormDTO sddto)
        {
            return _ads.getstudata(sddto);
        }

        [Route("getbuspassdata")]
        public Task<StudentBuspassFormDTO> getbuspassdata([FromBody]StudentBuspassFormDTO sddto)
        {
            return _ads.getbuspassdata(sddto);
        }
        [Route("academicload")]
        public StudentBuspassFormDTO academicload([FromBody]StudentBuspassFormDTO data)
        {
            return _ads.academicload(data);
        }
        [Route("getroutedata")]
        public StudentBuspassFormDTO getroutedata([FromBody]StudentBuspassFormDTO sddto)
        {
            return _ads.getroutedata(sddto);
        }

        [Route("getlocationdata")]
        public StudentBuspassFormDTO getlocationdata([FromBody]StudentBuspassFormDTO sddto)
        {
            return _ads.getlocationdata(sddto);
        }

        [Route("getlocationdataonly")]
        public StudentBuspassFormDTO getlocationdataonly([FromBody]StudentBuspassFormDTO sddto)
        {
            return _ads.getlocationdataonly(sddto);
        }

        [Route("savedata")]
        public StudentBuspassFormDTO savedata([FromBody]StudentBuspassFormDTO student)
        {
            return _ads.savedata(student);
        }

        [Route("paynow")]
        public StudentBuspassFormDTO paynow([FromBody] StudentBuspassFormDTO dt)
        {
            return _ads.paynow(dt);
        }
        [Route("paynow1")]
        public StudentBuspassFormDTO paynow1([FromBody] StudentBuspassFormDTO dt)
        {
            return _ads.paynow1(dt);
        }
        [Route("paynow2")]
        public StudentBuspassFormDTO paynow2([FromBody] StudentBuspassFormDTO dt)
        {
            return _ads.paynow2(dt);
        }

        [Route("getpaymentresponse/")]
        public PaymentDetails getpaymentresponse([FromBody]PaymentDetails response)
        {
            return _ads.payuresponse(response);
        }

        [Route("Razorpaypaymentresponse/")]
        public PaymentDetails Razorpaypaymentresponse([FromBody]PaymentDetails response)
        {
            return _ads.Razorpaypaymentresponse(response);
        }

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
    }
}
