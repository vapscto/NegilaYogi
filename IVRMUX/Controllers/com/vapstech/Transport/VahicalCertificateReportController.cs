using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Transport;
using PreadmissionDTOs.com.vaps.Transport;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Transport
{
    [Route("api/[controller]")]
    public class VahicalCertificateReportController : Controller
    {
        VahicalCertificateReportDelegate _driver = new VahicalCertificateReportDelegate();
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

        [Route("getdata/{id:int}")]
        public VahicalCertificateReportDTO getdata(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _driver.getdata(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [Route("savedata")]
        public VahicalCertificateReportDTO savedata([FromBody] VahicalCertificateReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _driver.savedata(data);
        }

        [Route("Onvahiclechange")]
        public VahicalCertificateReportDTO Onvahiclechange([FromBody] VahicalCertificateReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _driver.Onvahiclechange(data);
        }
        [Route("edit")]
        public VahicalCertificateReportDTO edit([FromBody] VahicalCertificateReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _driver.edit(data);
        }


        [Route("deleterecord/{id:int}")]
        public VahicalCertificateReportDTO deleterecord(int id )
        {
            VahicalCertificateReportDTO data = new VahicalCertificateReportDTO();
           data.TRVCT_Id = id;
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _driver.deleterecord(data);
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
