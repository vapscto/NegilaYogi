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
    public class CLGRoomMappingFacadeController : Controller
    {
        private readonly DomainModelMsSqlServerContext _db;
        public CLGRoomMappingInterface _ttcategory;

        public CLGRoomMappingFacadeController(DomainModelMsSqlServerContext db, CLGRoomMappingInterface maspag)
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
        public CLGRoomMappingDTO getorgdet([FromBody] CLGRoomMappingDTO data)
        {
            return _ttcategory.getdetails(data);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [Route("get_catg")]
        public CLGRoomMappingDTO get_catg([FromBody] CLGRoomMappingDTO org)
        {
            return _ttcategory.get_catg(org);
        }
        [Route("deactiveY")]
        public CLGRoomMappingDTO deactiveY([FromBody] CLGRoomMappingDTO org)
        {
            return _ttcategory.deactiveY(org);
        }

        [Route("getpossiblePeriod")]
        public CLGRoomMappingDTO getpossiblePeriod([FromBody] CLGRoomMappingDTO org)
        {
            return _ttcategory.getpossiblePeriod(org);
        }

        [Route("getdays")]
        public CLGRoomMappingDTO getdays([FromBody] CLGRoomMappingDTO org)
        {
            return _ttcategory.getdays(org);
        }
        [HttpPost]
        [Route("savedetail")]
        public CLGRoomMappingDTO Post([FromBody] CLGRoomMappingDTO org)
        {
            return _ttcategory.savedetail(org);
        }
        [HttpPost]
        [Route("editdata")]
        public CLGRoomMappingDTO editdata([FromBody] CLGRoomMappingDTO org)
        {
            return _ttcategory.editdata(org);
        }
        [HttpPost]
        [Route("get_roomfacility")]
        public CLGRoomMappingDTO get_roomfacility([FromBody] CLGRoomMappingDTO org)
        {
            return _ttcategory.get_roomfacility(org);
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
