using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.COE;
using corewebapi18072016.Delegates.com.vapstech.COE;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.COE
{
    [Route("api/[controller]")]
    public class COEController : Controller
    {
        COEdelegate deleg = new COEdelegate();
       [Route("getData/{id:int}")]
       public COEDTO getData(int id)
        {
            COEDTO data = new COEDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return deleg.getdata(data);
        }
        [Route("getEvents/{id:int}")]
        public COEDTO getEvents(int id)
        {
            COEDTO data = new COEDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.COEME_Id = id;
            return deleg.getEvents(data);
        }
        [Route("Sendmessage")]
        public COEDTO Sendmessage([FromBody]COEDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return deleg.sendmsg(data);
        }
        [Route("Confirmation/{id:int}")]
        public string Confirmation(int id)
        {
            return "Confirmed";
        }
    }
}
