using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using WebApplication1.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class BuspassFormFacade : Controller
    {
        public BuspassFormInterface _ads;

        public BuspassFormFacade(BuspassFormInterface adstu)
        {
            _ads = adstu;
        }

        [HttpPost]
        [Route("getloaddata")]
        public StudentHelthcertificateDTO getloaddata([FromBody]StudentHelthcertificateDTO data)
        {
            return _ads.getloaddata(data);
        }
        [HttpPost]
        [Route("getstudata")]
        public Task<StudentHelthcertificateDTO> getstudata([FromBody]StudentHelthcertificateDTO sddto)
        {
            return _ads.getstudata(sddto);
        }
        [HttpPost]
        [Route("getbuspassdata")]
        public Task<StudentHelthcertificateDTO> getbuspassdata([FromBody]StudentHelthcertificateDTO sddto)
        {
            return _ads.getbuspassdata(sddto);
        }
        [HttpPost]
        [Route("getroutedata")]
        public StudentHelthcertificateDTO getroutedata([FromBody]StudentHelthcertificateDTO sddto)
        {
            return _ads.getroutedata(sddto);
        }
        [HttpPost]
        [Route("getlocationdata")]
        public StudentHelthcertificateDTO getlocationdata([FromBody]StudentHelthcertificateDTO sddto)
        {
            return _ads.getlocationdata(sddto);
        }
        [HttpPost]
        [Route("savedata")]
        public StudentHelthcertificateDTO savedata([FromBody]StudentHelthcertificateDTO student)
        {
            return _ads.savedata(student);
        }

        [Route("paynow")]
        public StudentHelthcertificateDTO paynow([FromBody] StudentHelthcertificateDTO dt)
        {
            return _ads.paynow(dt);
        }

        [Route("getpaymentresponse/")]
        public PaymentDetails getpaymentresponse([FromBody]PaymentDetails response)
        {

            return _ads.payuresponse(response);
        }
    }
}
