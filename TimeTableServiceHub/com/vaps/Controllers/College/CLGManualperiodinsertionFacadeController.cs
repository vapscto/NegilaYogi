using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimeTableServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.TT;
using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeTableServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class CLGManualperiodinsertionFacadeController : Controller
    {
        private readonly DomainModelMsSqlServerContext _db;
        public CLGManualperiodinsertionInterface _ttcategory;

        public CLGManualperiodinsertionFacadeController(DomainModelMsSqlServerContext db, CLGManualperiodinsertionInterface maspag)
        {
            _db = db;
            _ttcategory = maspag;
        }

        // GET: api/valuesz
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [Route("getdetails")]
        public CLGManualperiodinsertionDTO getorgdet([FromBody] CLGManualperiodinsertionDTO data)
        {
            return _ttcategory.getdetails(data);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [Route("get_catg")]
        public CLGManualperiodinsertionDTO get_catg([FromBody] CLGManualperiodinsertionDTO org)
        {
            return _ttcategory.get_catg(org);
        }
        [Route("getclass_catg")]
        public CLGManualperiodinsertionDTO getclass_catg([FromBody] CLGManualperiodinsertionDTO org)
        {
            return _ttcategory.getclass_catg(org);
        }

        [Route("getpossiblePeriod")]
        public CLGManualperiodinsertionDTO getpossiblePeriod([FromBody] CLGManualperiodinsertionDTO org)
        {
            return _ttcategory.getpossiblePeriod(org);
        }

        [Route("getrpt")]
        public CLGManualperiodinsertionDTO getrpt([FromBody] CLGManualperiodinsertionDTO org)
        {
            return _ttcategory.getreport(org);
        }
        [HttpPost]
        [Route("savedetail")]
        public CLGManualperiodinsertionDTO Post([FromBody] CLGManualperiodinsertionDTO org)
        {
            return _ttcategory.savedetail(org);
        }
    
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
