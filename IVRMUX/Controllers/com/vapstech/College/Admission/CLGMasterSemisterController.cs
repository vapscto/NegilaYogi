using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;
using corewebapi18072016.Delegates.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Admission
{
   
    [Route("api/[controller]")]
    public class CLGMasterSemisterController : Controller
    {
        CLGMasterSemisterDelegete _sem = new CLGMasterSemisterDelegete();
        [HttpGet]
        public CLGMasterSemisterDTO getdata(int id)
        {
            CLGMasterSemisterDTO data = new CLGMasterSemisterDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _sem.getdata(data);
        }
        [Route("savesem")]
        public CLGMasterSemisterDTO save([FromBody]CLGMasterSemisterDTO data)
        {
            data.MI_Id= Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _sem.savesem(data);
        }
        [Route("editsem")]
        public CLGMasterSemisterDTO edit([FromBody]CLGMasterSemisterDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _sem.editsem(data);
        }
        [Route("activedeactivesem")]
        public CLGMasterSemisterDTO activedeactivesem([FromBody]CLGMasterSemisterDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _sem.activedeactivesem(data);

        }
        [Route("getOrder")]
        public CLGMasterSemisterDTO getOrder([FromBody]CLGMasterSemisterDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _sem.getOrder(data);

        }
        



    }
}
