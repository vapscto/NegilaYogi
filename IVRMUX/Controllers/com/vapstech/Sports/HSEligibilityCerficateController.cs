using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Sports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Sports
{
    [Route("api/[controller]")]
    public class HSEligibilityCerficateController : Controller
    {
        HSEligibilityCerficateDelegate delobj = new HSEligibilityCerficateDelegate();

        [Route("Getdetails")]
        public HSEligibilityCerficateDTO Getdetails(HSEligibilityCerficateDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.Getdetails(data);
        }

        [Route("get_class")]
        public HSEligibilityCerficateDTO get_class([FromBody]HSEligibilityCerficateDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.get_class(data);
        }
        [Route("get_section")]
        public HSEligibilityCerficateDTO get_section([FromBody]HSEligibilityCerficateDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.get_section(data);
        }
        [Route("get_student")]
        public HSEligibilityCerficateDTO get_student([FromBody]HSEligibilityCerficateDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.get_student(data);
        }

        [Route("get_age")]
        public HSEligibilityCerficateDTO get_age([FromBody]HSEligibilityCerficateDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.get_age(data);
        }
        [Route("get_certificate")]
        public HSEligibilityCerficateDTO get_certificate([FromBody]HSEligibilityCerficateDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.get_certificate(data);
        }
        [Route("get_PUcertificate")]
        public HSEligibilityCerficateDTO get_PUcertificate([FromBody]HSEligibilityCerficateDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.get_PUcertificate(data);
        }


    }
}
