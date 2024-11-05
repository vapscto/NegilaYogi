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

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class StudyCertificateController : Controller
    {
        StudyCertificateDelegate adsd = new StudyCertificateDelegate();
        // GET: api/values
        [HttpGet]
        [Route("getdata/{id:int}")]
        public StudycertificateDTO getinitialdata(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return adsd.getdetails(id);
        }


        [Route("Studdetails")]
        public StudycertificateDTO getStudData([FromBody] StudycertificateDTO stuDTO)
        {
            stuDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            stuDTO.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return adsd.GetStudDataById(stuDTO);
        }

        [HttpPost]
        [Route("getS")]
        public StudycertificateDTO getstudentlist([FromBody]StudycertificateDTO student)
        {
            student.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            student.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return adsd.getstudlist(student);
        }

        [Route("onacademicyearchange")]
        public StudycertificateDTO onacademicyearchange([FromBody] StudycertificateDTO data)
        {
            data.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return adsd.onacademicyearchange(data);
        }

        [Route("searchfilter")]
        public StudycertificateDTO searchfilter([FromBody] StudycertificateDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return adsd.searchfilter(data);
        }

        [Route("Studdetailsconduct")]
        public StudycertificateDTO Studdetailsconduct([FromBody] StudycertificateDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return adsd.Studdetailsconduct(data);
        }

        //SRKVS Endorsement
        [Route("searchfilterSRKVS")]
        public StudycertificateDTO searchfilterSRKVS([FromBody] StudycertificateDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return adsd.searchfilterSRKVS(data);
        }

        [Route("StuddetailsSRKVS")]
        public StudycertificateDTO StuddetailsSRKVS([FromBody] StudycertificateDTO stuDTO)
        {
            stuDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            stuDTO.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return adsd.GetStudDetailsById(stuDTO);
        }
    }
}
