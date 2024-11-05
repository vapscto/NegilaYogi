using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.admission;
using DataAccessMsSqlServerProvider;

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class BBKVCustomReportFacade : Controller
    {
        public BBKVCustomReportInterface _report;
        private readonly DomainModelMsSqlServerContext _db;
        public BBKVCustomReportFacade(BBKVCustomReportInterface IAtt, DomainModelMsSqlServerContext db)
        {
            _report = IAtt;
            _db = db;
        }
        // GET: api/values


        // [Route("getdetails/{id:int}")]
        [Route("getdetails/{id:int}")]
        public BBKVCustomReportDTO getdetails(int id)
        {
            return _report.getdetails(id);
        }


        [Route("getnameregno")]
        public BBKVCustomReportDTO getnameregno([FromBody] BBKVCustomReportDTO data)
        {
            return _report.getnameregno(data);
        }

        [Route("stdnamechange")]
        public BBKVCustomReportDTO stdnamechange([FromBody] BBKVCustomReportDTO data)
        {
            return _report.stdnamechange(data);
        }
        [Route("onclicktcperortemo")]
        public BBKVCustomReportDTO onclicktcperortemo([FromBody] BBKVCustomReportDTO data)
        {
            return _report.onclicktcperortemo(data);
        }
        [Route("getTcdetails")]
        public BBKVCustomReportDTO getTcdetails([FromBody] BBKVCustomReportDTO data)
        {
            return _report.getTcdetails(data);
        }
        [Route("getTcdetailsJNS")]
        public BBKVCustomReportDTO getTcdetailsJNS([FromBody] BBKVCustomReportDTO data)
        {
            return _report.getTcdetailsJNS(data);
        }

        [Route("get_JSHSTcdetails")]
        public BBKVCustomReportDTO get_JSHSTcdetails([FromBody] BBKVCustomReportDTO data)
        {
            return _report.get_JSHSTcdetails(data);
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
