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
    public class PeriodTimeSettingController : Controller
    {
        PeriodTimeSettingDelegate objdelegate = new PeriodTimeSettingDelegate();


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5    
        [HttpGet]
        [Route("getalldetails")]
        public TT_Master_Day_Period_TimeDTO Get([FromQuery] int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getdetails(id);
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [Route("deactive")]
        public TT_Master_Day_Period_TimeDTO deletepages([FromBody] TT_Master_Day_Period_TimeDTO id)
        {
            return objdelegate.deleterec(id);
        }
        [HttpPost]
        [Route("savedetail")]
        public TT_Master_Day_Period_TimeDTO savedetail([FromBody] TT_Master_Day_Period_TimeDTO categorypage)
        {
            Int32 PeriodTimeSettingID = 0;

            if (HttpContext.Session.GetString("PeriodTimeSettingID") != null)
            {
                PeriodTimeSettingID = Convert.ToInt32(HttpContext.Session.GetString("PeriodTimeSettingID"));
            }

            categorypage.TTMDPT_Id = PeriodTimeSettingID;
            HttpContext.Session.Remove("PeriodTimeSettingID");

            //categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail(categorypage);
        }
        [HttpGet]

        [Route("getpagedetails/{id:int}")]
        public TT_Master_Day_Period_TimeDTO getpagedetail(int id)
        {
            HttpContext.Session.SetString("PeriodTimeSettingID", id.ToString());

            return objdelegate.getpagedetails(id);

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
