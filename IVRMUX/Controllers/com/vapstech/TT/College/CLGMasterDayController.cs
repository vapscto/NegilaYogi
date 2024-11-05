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
    public class CLGMasterDayController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        CLGMasterDayDelegate ad = new CLGMasterDayDelegate();

        // GET: api/Academic/5
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public CLGMasterDayDTO Get([FromQuery] int id)
        {
            CLGMasterDayDTO data = new CLGMasterDayDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.getalldetails(data);
        }
        [HttpGet]
        [Route("editDay/{id:int}")]
        public CLGMasterDayDTO editDay(int id)
        {
            CLGMasterDayDTO data = new CLGMasterDayDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.TTMD_Id = id;
            return ad.editDay(data);
        }

        // POST: api/Academic
        [HttpPost]
        [Route("getBranch")]
        public CLGMasterDayDTO getBranch([FromBody] CLGMasterDayDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.getBranch(data);
        }
        [HttpPost]
        [Route("saveday")]
        public CLGMasterDayDTO saveday([FromBody] CLGMasterDayDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.saveday(data);
        }

        [HttpPost]
        [Route("savesemday")]
        public CLGMasterDayDTO savesemday([FromBody] CLGMasterDayDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.savesemday(data);
        }

        [HttpPost]
        [Route("daydeactive")]
        public CLGMasterDayDTO daydeactive([FromBody] CLGMasterDayDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.daydeactive(data);
        }
      
         [HttpPost]
        [Route("deactivecrsday")]
        public CLGMasterDayDTO deactivecrsday([FromBody] CLGMasterDayDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.deactivecrsday(data);
        }

        [Route("getorder")]
        public CLGMasterDayDTO getorder(CLGMasterDayDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.getorder(data);
        }
        [Route("saveorder")]
        public CLGMasterDayDTO saveorder([FromBody] CLGMasterDayDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.saveorder(data);
        }




    }
}
