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
    public class TrnsMonthEndReportController : Controller
    {
        TrnsMonthEndReportDelegate _area = new TrnsMonthEndReportDelegate();

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
        public TrnsMonthEndReportDTO getdata(int id)
        {
           
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _area.getdata(id);
        }
        [Route("getdata1/{id:int}")]
        public TrnsMonthEndReportDTO getdata1(int id)
        {

            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _area.getdata1(id);
        }

        [Route("savedata")]
        public TrnsMonthEndReportDTO savedata([FromBody] TrnsMonthEndReportDTO data)
        {
            data.MI_Id= Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _area.savedata(data);
        }

        [Route("savedata1")]
        public TrnsMonthEndReportDTO savedata1([FromBody] TrnsMonthEndReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _area.savedata1(data);
        }
        [Route("geteditdata")]
        public TrnsMonthEndReportDTO geteditdata([FromBody]TrnsMonthEndReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _area.geteditdata(data);
        }

        [Route("activedeactive")]
        public TrnsMonthEndReportDTO activedeactive([FromBody] TrnsMonthEndReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _area.activedeactive(data);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
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
