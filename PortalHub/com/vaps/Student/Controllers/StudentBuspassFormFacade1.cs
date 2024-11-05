using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using PortalHub.com.vaps.Student.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Student;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Student.Controllers
{
    [Route("api/[controller]")]
    public class StudentBuspassFormFacade1 : Controller
    {
        public StudentBuspassFormInterface _ads;

        public StudentBuspassFormFacade1(StudentBuspassFormInterface adstu)
        {
            _ads = adstu;
        }

        [HttpPost]
        [Route("getloaddata")]
        public StudentBuspassFormDTO getloaddata([FromBody]StudentBuspassFormDTO data)
        {
            return _ads.getloaddata(data);
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

        [Route("getpaymentresponse/")]
        public PaymentDetails getpaymentresponse([FromBody]PaymentDetails response)
        {
            return _ads.payuresponse(response);
        }
    }
}
