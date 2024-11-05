using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COEServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.COE;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace COEServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class CoeReportGraphFacade : Controller
    {
        CoeReportGraphInterface _coeinterface;
        public CoeReportGraphFacade(CoeReportGraphInterface coeinterface)
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
        
         [Route("getReport")]
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
