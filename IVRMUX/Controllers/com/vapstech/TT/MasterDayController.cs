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
    [Route("api/[controller]")]
    public class MasterDayController : Controller
    {

        MasterDayDelegate _delegateday = new MasterDayDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5

        [HttpGet]
        [Route("getalldetails")]
        public TT_Master_DayDTO Get([FromQuery] int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegateday.getdetails(id);
        }


        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }


        [HttpPost]
        [Route("savedetail")]
        public TT_Master_DayDTO savedetail([FromBody] TT_Master_DayDTO objday)
        {
            objday.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegateday.savedetail(objday);
        }

        [HttpPost]
        [Route("savedaydetail")]
        public TT_Master_DayDTO savedaydetail([FromBody] TT_Master_DayDTO periodpage)
        {
            periodpage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegateday.savedaydetail(periodpage);
        }


        [Route("getdetails/{id:int}")]
        public TT_Master_DayDTO getdetails(int id)
        {
            return _delegateday.getpagedetails(id);

        }
        [Route("getdaydetails/{id:int}")]
        public TT_Master_DayDTO getdaydetails(int id)
        {
            return _delegateday.getdaydetails(id);

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("deletepages/{id:int}")]
        public TT_Master_DayDTO deletepages(int id)
        {
            return _delegateday.deleterec(id);
        }
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost]
        [Route("deactivate")]
        public TT_Master_DayDTO deactvate([FromBody] TT_Master_DayDTO id)
        {
            return _delegateday.deactivate(id);
        }
        [HttpPost]
        [Route("deactivate1")]
        public TT_Master_DayDTO deactvate1([FromBody] TT_Master_DayDTO id)
        {
            return _delegateday.deactivate1(id);
        }

        [Route("getorder")]
        public TT_Master_DayDTO getorder(TT_Master_DayDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegateday.getorder(data);
        }
        [Route("saveorder")]
        public TT_Master_DayDTO saveorder([FromBody] TT_Master_DayDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegateday.saveorder(data);
        }


        [Route("getavdata")]
        public TT_Master_DayDTO getavdata(TT_Master_DayDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegateday.getavdata(data);
        }
        [Route("getPeriods")]
        public TT_Master_DayDTO getPeriods( [FromBody]TT_Master_DayDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegateday.getPeriods(data);
        }
        [Route("allocateperiod")]
        public TT_Master_DayDTO allocateperiod( [FromBody]TT_Master_DayDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegateday.allocateperiod(data);
        }

      

    }
}
