using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.admission;
using DataAccessMsSqlServerProvider;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class HHSTCCustomReportFacadeController : Controller
    {
        public HHSTCCustomReportInterface _report;
        private readonly DomainModelMsSqlServerContext _db;
        public HHSTCCustomReportFacadeController(HHSTCCustomReportInterface IAtt, DomainModelMsSqlServerContext db)
        {
            _report = IAtt;
            _db = db;
        }
        // GET: api/values


        // [Route("getdetails/{id:int}")]
        [Route("getdetails/{id:int}")]
        public HHSTCCustomReportDTO getdetails(int id)
        {
            return _report.getdetails(id);
        }


        [Route("getnameregno")]
        public HHSTCCustomReportDTO getnameregno([FromBody] HHSTCCustomReportDTO data)
        {
            return _report.getnameregno(data);
        }

        [Route("stdnamechange")]
        public HHSTCCustomReportDTO stdnamechange([FromBody] HHSTCCustomReportDTO data)
        {
            return _report.stdnamechange(data);
        }
        [Route("onclicktcperortemo")]
        public HHSTCCustomReportDTO onclicktcperortemo([FromBody] HHSTCCustomReportDTO data)
        {
            return _report.onclicktcperortemo(data);
        }
        [Route("getTcdetails")]
        public HHSTCCustomReportDTO getTcdetails([FromBody] HHSTCCustomReportDTO data)
        {
            return _report.getTcdetails(data);
        }

        [Route("Vikasha_getTcdetails")]
        public HHSTCCustomReportDTO Vikasha_getTcdetails([FromBody] HHSTCCustomReportDTO data)
        {
            return _report.Vikasha_getTcdetails(data);
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
