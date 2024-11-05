using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.TT;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.TT;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.TT
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class CLGDeputationController : Controller
    {
        CLGDeputationDelegate objdelegate = new CLGDeputationDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5    
        [HttpGet]
        [Route("getdetails")]
        public CLGDeputationDTO Get([FromQuery] int id)
        {
            CLGDeputationDTO data = new CLGDeputationDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getdetails(data);
        }
       
        [HttpPost]
        [Route("savedetails")]
        public CLGDeputationDTO savedetails([FromBody] CLGDeputationDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetails(data);
        }

        [HttpPost]
        [Route("get_period_alloted")]
        public CLGDeputationDTO get_period_alloted([FromBody] CLGDeputationDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.get_period_alloted(data);
        }
        [HttpPost]
        [Route("get_free_stfdets")]
        public CLGDeputationDTO get_free_stfdets([FromBody] CLGDeputationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.get_free_stfdets(data);
        }
        [HttpPost]
        [Route("getalldetailsviewrecords2")]
        public CLGDeputationDTO getalldetailsviewrecords2([FromBody] CLGDeputationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getalldetailsviewrecords2(data);
        }

        [HttpPost]
        [Route("viewdeputation")]
        public CLGDeputationDTO viewdeputation([FromBody] CLGDeputationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.viewdeputation(data);
        }
        [HttpPost]
        [Route("viewabsent")]
        public CLGDeputationDTO viewabsent([FromBody] CLGDeputationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.viewabsent(data);
        }

        [HttpPost]
        [Route("getabsentstaff")]
        public CLGDeputationDTO getabsentstaff([FromBody] CLGDeputationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getabsentstaff(data);
        }

    }
}
