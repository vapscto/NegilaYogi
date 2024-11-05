using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.COE;
using CoeServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CoeServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class COEReportFacadeController : Controller
    {
        COEReportInterface _coeinterface;
        public COEReportFacadeController(COEReportInterface coeinterface)
        {
            _coeinterface = coeinterface;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [Route("getdata/{id:int}")]
        public async Task<COEReportDTO> getdata(int id)
        {
            return await _coeinterface.getinitialData(id);
        }

        // POST api/values
        [HttpPost]
        public COEReportDTO getReport([FromBody]COEReportDTO data)
        {
            return _coeinterface.getReport(data);
        }

        // PUT api/values/5
        [Route("mothreport")]
        public COEReportDTO mothreport([FromBody]COEReportDTO data)
        {
            return _coeinterface.mothreport(data);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
