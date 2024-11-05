using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using WebApplication1.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class SmartCardFreezeController : Controller
    {
        public SmartCardFreezeInterface _report;
        public SmartCardFreezeController(SmartCardFreezeInterface _screport)
        {
            _report = _screport;
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
        [Route("getdetails")]
        public SmartCardFreezeDTO getdetails([FromBody]SmartCardFreezeDTO dd)
        {
            return _report.getdetails(dd);
        }
        [Route("getstddetails")]
        public SmartCardFreezeDTO getstddetails([FromBody]SmartCardFreezeDTO dd)
        {
            return _report.getstddetails(dd);
        }

        [Route("getdetailsstf")]
        public SmartCardFreezeDTO getdetailsstf([FromBody]SmartCardFreezeDTO dd)
        {
            return _report.getdetailsstf(dd);
        }
        [Route("getdetailsstfdes")]
        public SmartCardFreezeDTO getdetailsstfdes([FromBody]SmartCardFreezeDTO dd)
        {
            return _report.getdetailsstfdes(dd);
        }
        [Route("depchange")]
        public SmartCardFreezeDTO depchange([FromBody]SmartCardFreezeDTO dd)
        {
            return _report.depchange(dd);
        }
        [Route("getstfdetails")]
        public SmartCardFreezeDTO getstfdetails([FromBody]SmartCardFreezeDTO dd)
        {
            return _report.getstfdetails(dd);
        }
        [Route("getdetailsCLG")]
        public SmartCardFreezeDTO getdetailsCLG([FromBody]SmartCardFreezeDTO dd)
        {
            return _report.getdetailsCLG(dd);
        }
         [Route("getstddetailscld")]
        public SmartCardFreezeDTO getstddetailscld([FromBody]SmartCardFreezeDTO dd)
        {
            return _report.getstddetailscld(dd);
        }


        [Route("admsearch")]
        public SmartCardFreezeDTO admsearch([FromBody]SmartCardFreezeDTO dd)
        {
            return _report.admsearch(dd);
        }
         [Route("admsearchclg")]
        public SmartCardFreezeDTO admsearchclg([FromBody]SmartCardFreezeDTO dd)
        {
            return _report.admsearchclg(dd);
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
