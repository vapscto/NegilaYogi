using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;
using corewebapi18072016.Delegates.com.vapstech.FrontOffice;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.FrontOffice
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterHolidayController : Controller
    {
        MasterHolidayDelegate dele = new MasterHolidayDelegate();
        // GET: api/values
        [HttpGet("{id:int}")]
        public MasterHolidayDTO GetData(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.getdata(id);
        }
       
        [Route("savedata")]
        // GET api/values/5
        public MasterHolidayDTO savedata([FromBody]MasterHolidayDTO categorypage)
        {
            categorypage.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.savedata(categorypage);
        }

        [Route("Change")]
        // GET api/values/5
        public MasterHolidayDTO Change([FromBody]MasterHolidayDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.Change(categorypage);
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [Route("delete")]
        public MasterHolidayDTO delete_data([FromBody]MasterHolidayDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.delete_data(categorypage);
        }

        [Route("advloaddata/{id:int}")]
        public MasterHolidayDTO advloaddata(int id)
        {
            MasterHolidayDTO obj = new MasterHolidayDTO();
            obj.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.advloaddata(obj);
        }

        [Route("saveadvmasterHolidaydata")]
        public MasterHolidayDTO saveadvmasterHolidaydata([FromBody]MasterHolidayDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.LogInId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return dele.saveadvmasterHolidaydata(categorypage);
        }

        [Route("advdelete/{id:int}")]
        public MasterHolidayDTO advdelete(int id)
        {
            return dele.advdelete(id);
        }
        
        [Route("editadvmasterHoliday")]
        public MasterHolidayDTO editadvmasterHoliday ([FromBody] MasterHolidayDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LogInId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return dele.editadvmasterHoliday(data);
        }
    }
}
