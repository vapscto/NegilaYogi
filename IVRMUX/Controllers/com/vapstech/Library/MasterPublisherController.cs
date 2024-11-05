using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Library
{
    [Route("api/[controller]")]
    public class MasterPublisherController : Controller
    {

        MasterPublisherDelegate _delobj = new MasterPublisherDelegate();

        
       [Route("Savedata")]
       public MasterPublisherDTO Savedata([FromBody]MasterPublisherDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _delobj.Savedata(data);
        }
        [Route("getdetails/{id:int}")]
        public MasterPublisherDTO getdetails(int id)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            id = mi_id;
            return _delobj.getdetails(id);
        }
        [Route("deactiveY")]
        public MasterPublisherDTO deactiveY([FromBody]MasterPublisherDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _delobj.deactiveY(data);
        }
    }
}
