using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.TT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.TT;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.TT.College
{
    [Route("api/[controller]")]
    public class DeputationNewController : Controller
    {
        DeputationNewDelegate objdelegate = new DeputationNewDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5    
        [HttpGet]
        [Route("getdetails")]
        public TTDeputationDTO Get([FromQuery] int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getdetails(id);
        }

        [HttpPost]
        [Route("savedetails")]
        public TTDeputationDTO savedetails([FromBody] TTDeputationDTO categorypage)
        {
            // categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetails(categorypage);
        }

        [HttpPost]
        [Route("get_period_alloted")]
        public TTDeputationDTO get_period_alloted([FromBody] TTDeputationDTO categorypage)
        {

            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.get_period_alloted(categorypage);
        }
        [HttpPost]
        [Route("get_free_stfdets")]
        public TTDeputationDTO get_free_stfdets([FromBody] TTDeputationDTO categorypage)
        {

            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.get_free_stfdets(categorypage);
        }
        [HttpPost]
        [Route("getalldetailsviewrecords2")]
        public TTDeputationDTO getalldetailsviewrecords2([FromBody] TTDeputationDTO categorypage)
        {

            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getalldetailsviewrecords2(categorypage);
        }

        [HttpPost]
        [Route("viewrecordspopup9")]
        public TTDeputationDTO viewrecordspopup9([FromBody] TTDeputationDTO categorypage)
        {

            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.viewrecordspopup9(categorypage);
        }


        [HttpPost]
        [Route("viewdeputation")]
        public TTDeputationDTO viewdeputation([FromBody] TTDeputationDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.viewdeputation(categorypage);
        }

        [HttpPost]
        [Route("viewabsent")]
        public TTDeputationDTO viewabsent([FromBody] TTDeputationDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.viewabsent(categorypage);
        }
        [HttpPost]
        [Route("getabsentstaff")]
        public TTDeputationDTO getabsentstaff([FromBody] TTDeputationDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getabsentstaff(categorypage);
        }

    }
}
