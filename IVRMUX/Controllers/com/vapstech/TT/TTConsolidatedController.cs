using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.TT;
using PreadmissionDTOs.com.vaps.TT;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.TT
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class TTConsolidatedController : Controller
    {
        TTConsolidatedDelegate objdelegate = new TTConsolidatedDelegate();


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5    
        [HttpGet]
        [Route("getalldetails")]
        public TTConsolidatedDTO Get([FromQuery] int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getalldetails(id);
        }
        [HttpPost]
        [Route("getclass_catg")]
        public TTConsolidatedDTO getclass_catg([FromBody] TTConsolidatedDTO catgpage)
        {
            // categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            catgpage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getclass_catg(catgpage);
        }
        [HttpPost]
        [Route("getrpt")]
        public TTConsolidatedDTO getrpt([FromBody] TTConsolidatedDTO categorypage)
        { 
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getrpt(categorypage);
        }

       

    }
}
