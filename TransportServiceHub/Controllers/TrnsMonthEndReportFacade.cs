using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TransportServiceHub.Interfaces;
using PreadmissionDTOs.com.vaps.Transport;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TransportServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class TrnsMonthEndReportFacade : Controller
    {
        public TrnsMonthEndReportInterface _areaint;

        public TrnsMonthEndReportFacade(TrnsMonthEndReportInterface areaz)
        {
            _areaint = areaz;
        }
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


        [Route("getdata1/{id:int}")]
        public TrnsMonthEndReportDTO getdata1(int id)
        {

            return _areaint.getdata1(id);
        }

        [Route("savedata1")]
        public TrnsMonthEndReportDTO savedata1([FromBody]TrnsMonthEndReportDTO data)
        {
            return _areaint.savedata1(data);
        }
        [Route("getdata/{id:int}")]
        public TrnsMonthEndReportDTO getdata(int id)
        {

            return _areaint.getdata(id);
        }

        [Route("savedata")]
        public TrnsMonthEndReportDTO savedata([FromBody]TrnsMonthEndReportDTO data)
        {
            return _areaint.savedata(data);
        }
        [Route("geteditdata")]
        public TrnsMonthEndReportDTO geteditdata([FromBody] TrnsMonthEndReportDTO data)
        {
            return _areaint.geteditdata(data);
        }
        [Route("activedeactive")]
        public TrnsMonthEndReportDTO activedeactive([FromBody] TrnsMonthEndReportDTO data)
        {
            return _areaint.activedeactive(data);
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
