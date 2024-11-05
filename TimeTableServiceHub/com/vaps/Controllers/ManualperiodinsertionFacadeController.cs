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
    public class ManualperiodinsertionFacadeController : Controller
    {
        private readonly DomainModelMsSqlServerContext _db;
        public ManualperiodinsertionInterface _ttcategory;

        public ManualperiodinsertionFacadeController(DomainModelMsSqlServerContext db, ManualperiodinsertionInterface maspag)
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
        [Route("getdetails/{id:int}")]
        public ManualperiodinsertionDTO getorgdet(int id)
        {
            return _ttcategory.getdetails(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [Route("get_catg")]
        public ManualperiodinsertionDTO get_catg([FromBody] ManualperiodinsertionDTO org)
        {
            return _ttcategory.get_catg(org);
        }
        [Route("getclass_catg")]
        public ManualperiodinsertionDTO getclass_catg([FromBody] ManualperiodinsertionDTO org)
        {
            return _ttcategory.getclass_catg(org);
        }

        [Route("getpossiblePeriod")]
        public ManualperiodinsertionDTO getpossiblePeriod([FromBody] ManualperiodinsertionDTO org)
        {
            return _ttcategory.getpossiblePeriod(org);
        }

        [Route("getrpt")]
        public ManualperiodinsertionDTO getrpt([FromBody] ManualperiodinsertionDTO org)
        {
            return _ttcategory.getreport(org);
        }
        [HttpPost]
        [Route("savedetail")]
        public ManualperiodinsertionDTO Post([FromBody] ManualperiodinsertionDTO org)
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
