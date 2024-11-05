using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Chairman.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
//using AdmissionServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Chairman.Controllers
{


    [Route("api/[controller]")]
    public class Ch_feedbackFacadeController : Controller
    {
        public Ch_feedbackInterface _ChairmanDashboardReport;

        public Ch_feedbackFacadeController(Ch_feedbackInterface data)
        {
            _ChairmanDashboardReport = data;
        }


        [Route("getdata")]
        public Ch_feedbackDTO getdata([FromBody] Ch_feedbackDTO obj)
        {
            return _ChairmanDashboardReport.getdata(obj);
        }

       

        [Route("onmonth")]
        public Ch_feedbackDTO onmonth([FromBody] Ch_feedbackDTO data)
        {
            return _ChairmanDashboardReport.onmonth(data);
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
