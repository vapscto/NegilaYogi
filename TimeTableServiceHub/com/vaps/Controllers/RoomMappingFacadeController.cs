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
    public class RoomMappingFacadeController : Controller
    {
        private readonly DomainModelMsSqlServerContext _db;
        public RoomMappingInterface _ttcategory;

        public RoomMappingFacadeController(DomainModelMsSqlServerContext db, RoomMappingInterface maspag)
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
        public RoomMappingDTO getorgdet([FromBody] RoomMappingDTO data)
        {
            return _ttcategory.getdetails(data);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [Route("get_catg")]
        public RoomMappingDTO get_catg([FromBody] RoomMappingDTO org)
        {
            return _ttcategory.get_catg(org);
        }
        [Route("deactiveY")]
        public RoomMappingDTO deactiveY([FromBody] RoomMappingDTO org)
        {
            return _ttcategory.deactiveY(org);
        }

        [Route("getpossiblePeriod")]
        public RoomMappingDTO getpossiblePeriod([FromBody] RoomMappingDTO org)
        {
            return _ttcategory.getpossiblePeriod(org);
        }

        [Route("getrpt")]
        public RoomMappingDTO getrpt([FromBody] RoomMappingDTO org)
        {
            return _ttcategory.getreport(org);
        }
        [HttpPost]
        [Route("savedetail")]
        public RoomMappingDTO Post([FromBody] RoomMappingDTO org)
        {
            return _ttcategory.savedetail(org);
        }
        [HttpPost]
        [Route("editdata")]
        public RoomMappingDTO editdata([FromBody] RoomMappingDTO org)
        {
            return _ttcategory.editdata(org);
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
