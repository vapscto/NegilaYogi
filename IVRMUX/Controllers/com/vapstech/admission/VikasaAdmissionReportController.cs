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
using corewebapi18072016.Delegates.com.vapstech.admission;

namespace corewebapi18072016.Controllers.com.vapstech.admission
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class VikasaAdmissionReportController : Controller
    {

        VikasaAdmissionReportDelegate adsd = new VikasaAdmissionReportDelegate();
        // GET: api/values
        [HttpGet]
        [Route("getdata/{id:int}")]
        public VikasaAdmissionreportDTO getinitialdata(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return adsd.getdetails(id);
        }

        [HttpPost]
        [Route("getstudbyclass")]
        public VikasaAdmissionreportDTO getStudDatabyclass([FromBody] VikasaAdmissionreportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return adsd.getStudDatabyclass(data);
        }


        [Route("Studdetails")]
        public VikasaAdmissionreportDTO getStudData([FromBody] VikasaAdmissionreportDTO stuDTO)
        {
            stuDTO.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            stuDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return adsd.GetStudDataById(stuDTO);
        }

        [Route("onacademicyearchange")]
        public VikasaAdmissionreportDTO onacademicyearchange([FromBody] VikasaAdmissionreportDTO data)
        {
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return adsd.onacademicyearchange(data);
        }
        [Route("searchfilter")]
        public VikasaAdmissionreportDTO searchfilter([FromBody] VikasaAdmissionreportDTO data)
        {
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return adsd.searchfilter(data);
        }

        [Route("ShowReport")]
        public VikasaAdmissionreportDTO ShowReport([FromBody] VikasaAdmissionreportDTO data)
        {
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return adsd.ShowReport(data);
        }
        [Route("ShowReport1")]
        public VikasaAdmissionreportDTO ShowReport1([FromBody] VikasaAdmissionreportDTO data)
        {
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return adsd.ShowReport1(data);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }
    }
}