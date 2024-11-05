using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs.com.vaps.TT;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.TT.College;


namespace IVRMUX.Controllers.com.vapstech.TT.College
{
    [Route("api/[controller]")]
    public class CLGBreakTimeSettingController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        CLGBreakTimeSettingDelegate ad = new CLGBreakTimeSettingDelegate();

        // GET: api/Academic/5
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public CLGBreakTimeSettingDTO Get([FromQuery] int id)
        {
            CLGBreakTimeSettingDTO data = new CLGBreakTimeSettingDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.getalldetails(data);
        }
        [HttpGet]
        [Route("editDay/{id:int}")]
        public CLGBreakTimeSettingDTO editDay(int id)
        {
            CLGBreakTimeSettingDTO data = new CLGBreakTimeSettingDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.TTMD_Id = id;
            return ad.editDay(data);
        }

        // POST: api/Academic
        [HttpPost]
        [Route("getBranch")]
        public CLGBreakTimeSettingDTO getBranch([FromBody] CLGBreakTimeSettingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.getBranch(data);
        }
        [HttpPost]
        [Route("savetimedetail")]
        public CLGBreakTimeSettingDTO savetimedetail([FromBody] CLGBreakTimeSettingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.savetimedetail(data);
        }

        [HttpPost]
        [Route("getmaximumperiodscount")]
        public CLGBreakTimeSettingDTO getmaximumperiodscount([FromBody] CLGBreakTimeSettingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.getmaximumperiodscount(data);
        }

        [HttpPost]
        [Route("deactivate")]
        public CLGBreakTimeSettingDTO deactivate([FromBody] CLGBreakTimeSettingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.deactivate(data);
        }
      
        
        [Route("geteditdetails/{id:int}")]
        public CLGBreakTimeSettingDTO geteditdetails(int id)
        {
            CLGBreakTimeSettingDTO data = new CLGBreakTimeSettingDTO();
            data.TTMBC_Id = id;
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.geteditdetails(data);
        }
      

      


    }
}
