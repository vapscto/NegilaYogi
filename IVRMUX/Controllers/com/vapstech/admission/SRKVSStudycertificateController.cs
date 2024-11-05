using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model;
using PreadmissionDTOs;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using PreadmissionDTOs.com.vaps.admission;
using IVRMUX.Delegates.com.vapstech.admission;

namespace IVRMUX.Controllers.com.vapstech.admission
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]    
    public class SRKVSStudycertificateController : Controller
    {
        SRKVSStudyCertificateDelegate adsd = new SRKVSStudyCertificateDelegate();
        // GET: api/values
        [HttpGet]
        [Route("getdata/{id:int}")]
        public SRKVSStudycertificateDTO getinitialdata(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return adsd.getdetails(id);
        }

        [Route("Studdetails")]
        public SRKVSStudycertificateDTO getStudData([FromBody] SRKVSStudycertificateDTO stuDTO)
        {
            stuDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            stuDTO.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));             
            return adsd.GetStudDataById(stuDTO);
        }

        [HttpPost]
        [Route("getS")]
        public SRKVSStudycertificateDTO getstudentlist([FromBody]SRKVSStudycertificateDTO student)
        {
            student.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            student.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return adsd.getstudlist(student);
        }

        [Route("onacademicyearchange")]
        public SRKVSStudycertificateDTO onacademicyearchange([FromBody] SRKVSStudycertificateDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return adsd.onacademicyearchange(data);
        }

        [Route("searchfilter")]
        public SRKVSStudycertificateDTO searchfilter([FromBody] SRKVSStudycertificateDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return adsd.searchfilter(data);
        }

        [Route("Studdetailsconduct")]
        public SRKVSStudycertificateDTO Studdetailsconduct([FromBody] SRKVSStudycertificateDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return adsd.Studdetailsconduct(data);
        }        
    }
}
