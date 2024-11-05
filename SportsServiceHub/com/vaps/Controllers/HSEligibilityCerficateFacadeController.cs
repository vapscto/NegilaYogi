using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;
using SportsServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SportsServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class HSEligibilityCerficateFacadeController : Controller
    {
        public HSEligibilityCerficateInterface interobj;
        public HSEligibilityCerficateFacadeController(HSEligibilityCerficateInterface inter)
        {
            interobj = inter;
        }

        [Route("Getdetails")]
        public HSEligibilityCerficateDTO Getdetails([FromBody]HSEligibilityCerficateDTO data)
        {
            return interobj.Getdetails(data);
        }
        [Route("get_class")]
        public HSEligibilityCerficateDTO get_class([FromBody]HSEligibilityCerficateDTO data)
        {
            return interobj.get_class(data);
        }
        [Route("get_section")]
        public HSEligibilityCerficateDTO get_section([FromBody]HSEligibilityCerficateDTO data)
        {
            return interobj.get_section(data);
        }
        [Route("get_student")]
        public HSEligibilityCerficateDTO get_student([FromBody]HSEligibilityCerficateDTO data)
        {
            return interobj.get_student(data);
        }
        [Route("get_age")]
        public HSEligibilityCerficateDTO get_age([FromBody]HSEligibilityCerficateDTO data)
        {
            return interobj.get_age(data);
        }
        [Route("get_certificate")]
        public HSEligibilityCerficateDTO get_certificate([FromBody]HSEligibilityCerficateDTO data)
        {
            return interobj.get_certificate(data);
        }
        [Route("get_PUcertificate")]
        public HSEligibilityCerficateDTO get_PUcertificate([FromBody]HSEligibilityCerficateDTO data)
        {
            return interobj.get_PUcertificate(data);
        }
    }
}
