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
    public class CLGStaffReplacementInSectionFacadeController : Controller
    {
        private readonly DomainModelMsSqlServerContext _db;
        public CLGStaffReplacementInSectionInterface _ttcategory;

        public CLGStaffReplacementInSectionFacadeController(DomainModelMsSqlServerContext db, CLGStaffReplacementInSectionInterface maspag)
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
        public CLGStaffReplacementInSectionDTO getorgdet([FromBody] CLGStaffReplacementInSectionDTO org)
        {
            return _ttcategory.getdetails(org);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [Route("get_catg")]
        public CLGStaffReplacementInSectionDTO get_catg([FromBody] CLGStaffReplacementInSectionDTO org)
        {
            return _ttcategory.get_catg(org);
        }
        [Route("getclass_catg")]
        public CLGStaffReplacementInSectionDTO getclass_catg([FromBody] CLGStaffReplacementInSectionDTO org)
        {
            return _ttcategory.getclass_catg(org);
        }

        [Route("getpossiblePeriod")]
        public CLGStaffReplacementInSectionDTO getpossiblePeriod([FromBody] CLGStaffReplacementInSectionDTO org)
        {
            return _ttcategory.getpossiblePeriod(org);
        }

        [Route("getrpt")]
        public CLGStaffReplacementInSectionDTO getrpt([FromBody] CLGStaffReplacementInSectionDTO org)
        {
            return _ttcategory.getreport(org);
        }
        [HttpPost]
        [Route("savedetail")]
        public CLGStaffReplacementInSectionDTO Post([FromBody] CLGStaffReplacementInSectionDTO org)
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
