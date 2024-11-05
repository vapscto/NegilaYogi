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
    public class CLGStudentRouteMappingFacadeController : Controller
    {
        public CLGStudentRouteMappingInterface _areaint;

        public CLGStudentRouteMappingFacadeController(CLGStudentRouteMappingInterface areaz)
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
        [Route("getdata")]
        public CLGStudentRouteMappingDTO getdata([FromBody] CLGStudentRouteMappingDTO data)
        {

            return _areaint.getdata(data);
        }

        [Route("savedata")]
        public CLGStudentRouteMappingDTO savedata([FromBody] CLGStudentRouteMappingDTO data)
        {
            return _areaint.savedata(data);
        }
        [Route("geteditdata")]
        public CLGStudentRouteMappingDTO geteditdata([FromBody] CLGStudentRouteMappingDTO data)
        {
            return _areaint.geteditdata(data);
        }
        [Route("getstudents")]
        public CLGStudentRouteMappingDTO getstudents([FromBody] CLGStudentRouteMappingDTO data)
        {
            return _areaint.getstudents(data);
        }
        [Route("checkduplicateno")]
        public CLGStudentRouteMappingDTO checkduplicateno([FromBody] CLGStudentRouteMappingDTO data)
        {
            return _areaint.checkduplicateno(data);
        }
        [Route("viewrecordspopup")]
        public CLGStudentRouteMappingDTO viewrecordspopup([FromBody] CLGStudentRouteMappingDTO data)
        {
            return _areaint.viewrecordspopup(data);
        }
        [Route("getreportedit")]
        public CLGStudentRouteMappingDTO getreportedit([FromBody] CLGStudentRouteMappingDTO data)
        {
            return _areaint.getreportedit(data);
        }
        [Route("getreporteditbuspass")]
        public CLGStudentRouteMappingDTO getreporteditbuspass([FromBody] CLGStudentRouteMappingDTO data)
        {
            return _areaint.getreporteditbuspass(data);
        }
        [Route("savedatabuspass")]
        public CLGStudentRouteMappingDTO savedatabuspass([FromBody] CLGStudentRouteMappingDTO data)
        {
            return _areaint.savedatabuspass(data);
        }

        [Route("deactivate")]
        public CLGStudentRouteMappingDTO deactivate([FromBody] CLGStudentRouteMappingDTO data)
        {
            return _areaint.deactivate(data);
        }
        [Route("SearchByColumn")]
        public CLGStudentRouteMappingDTO SearchByColumn([FromBody] CLGStudentRouteMappingDTO data)
        {
            return _areaint.SearchByColumn(data);
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
