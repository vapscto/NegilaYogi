using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.College.Admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class MasterBatchController : Controller
    {
        // GET: api/values
        MasterBatchDelegete _batch = new MasterBatchDelegete();
        [HttpGet]
        public AdmCollegeMasterBatchDTO getdata(int id)
        {
            AdmCollegeMasterBatchDTO data = new AdmCollegeMasterBatchDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _batch.getdata(data);
        }
        [Route("savebatch")]
        public AdmCollegeMasterBatchDTO save([FromBody]AdmCollegeMasterBatchDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _batch.savebatch(data);
        }
        [Route("editbatch")]
        public AdmCollegeMasterBatchDTO edit([FromBody]AdmCollegeMasterBatchDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _batch.editbatch(data);
        }
        [Route("activedeactivebatch")]
        public AdmCollegeMasterBatchDTO activedeactivebatch([FromBody]AdmCollegeMasterBatchDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _batch.activedeactivebatch(data);

        }
    }
}
