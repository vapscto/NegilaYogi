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
    public class KMLogBookController : Controller
    {
        KMLogBookDelegate _driver = new KMLogBookDelegate();
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
        public KMLogBookDTO getdata(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _driver.getdata(id);
        }
         [Route("getreportdata/{id:int}")]
        public KMLogBookDTO getreportdata(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _driver.getreportdata(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [Route("savedata")]
        public KMLogBookDTO savedata([FromBody] KMLogBookDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _driver.savedata(data);
        }
        [Route("getkmreport")]
        public KMLogBookDTO getkmreport([FromBody] KMLogBookDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _driver.getkmreport(data);
        }

        [Route("Onvahiclechange")]
        public KMLogBookDTO Onvahiclechange([FromBody] KMLogBookDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _driver.Onvahiclechange(data);
        }
        [Route("vehicletypechange")]
        public KMLogBookDTO vehicletypechange([FromBody] KMLogBookDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _driver.vehicletypechange(data);
        }
        [Route("edit")]
        public KMLogBookDTO edit([FromBody] KMLogBookDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _driver.edit(data);
        }

        [Route("deleterecord")]
        public KMLogBookDTO deleterecord([FromBody] KMLogBookDTO data)
        {
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
