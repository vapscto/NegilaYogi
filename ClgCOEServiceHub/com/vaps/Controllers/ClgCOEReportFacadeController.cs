using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClgCOEServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;

using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.College.COE;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ClgCOEServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ClgCOEReportFacadeController : Controller
    {
        public ClgCOEReportInterface _ttcategory;

        public ClgCOEReportFacadeController(ClgCOEReportInterface maspag)
        {
            _ttcategory = maspag;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        //[Route("getdetails")]
        //public ClgMasterCOEDTO getdetails([FromBody] ClgMasterCOEDTO org)
        //{
        //    return _ttcategory.getdetails(org);
        //}
        [Route("getdata/{id:int}")]
        public ClgMasterCOEDTO getdata(int id)
        {
            return _ttcategory.getinitialData(id);
        }

        // POST api/values
        [HttpPost]
        [Route("getReport")]
        public Task<ClgMasterCOEDTO> getReport([FromBody]ClgMasterCOEDTO data)
        {
            return _ttcategory.getReport(data);
        }

        // PUT api/values/5
        [Route("mothreport")]
        public ClgMasterCOEDTO mothreport([FromBody]ClgMasterCOEDTO data)
        {
            return _ttcategory.mothreport(data);
        }
    }
}
