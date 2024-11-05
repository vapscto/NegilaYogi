using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class GeneralConfigController : Controller
    {
        GeneralConfigDelegate _congfigdelegate= new GeneralConfigDelegate();
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
        [HttpGet]
        [Route("Configurationget/{id:int}")]
        public GeneralConfigDTO Configurationget(int id)
        {
            GeneralConfigDTO data = new GeneralConfigDTO();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            return _congfigdelegate.Configurationget(data);
        }
        [Route("geteditdata/{id:int}")]
        public GeneralConfigDTO geteditdata(int id)
        {
            GeneralConfigDTO data = new GeneralConfigDTO();
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;
            data.IVRMGC_Id = id;
            return _congfigdelegate.geteditdata(data);
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [HttpPost]
        [Route("savegenConfigData")]
        public GeneralConfigDTO savegenConfigData([FromBody] GeneralConfigDTO mstConfig)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            mstConfig.MI_Id = mid;
            return _congfigdelegate.savegenConfigData(mstConfig);
        }


        [HttpPost]
        [Route("getcontent")]
        public GeneralConfigDTO getcontent([FromBody] GeneralConfigDTO mstConfig)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            mstConfig.MI_Id = mid;
            return _congfigdelegate.getcontent(mstConfig);
        }
        [Route("deleteUserNameconfig/{id:int}")]
        public GeneralConfigDTO deleteRollnoconfig(int id)
        {
            GeneralConfigDTO mand = new GeneralConfigDTO();
            int moid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MO_Id"));
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            mand.MI_Id = mid;
            mand.IVRMCUNP_Id = id;

            return _congfigdelegate.deleteUserNameconfig(mand);
        }
    }
}
