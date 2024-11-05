using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.admission;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.com.vaps.admission.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class SMSMasterApprovalController : Controller
    {
        SMSMasterApprovalDelegate crStr = new SMSMasterApprovalDelegate();

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet]
        [Route("Getdetails")]
        public SMSMasterApprovalDTO Getdetails(SMSMasterApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            // data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            // data.MI_Id = 5;
            return crStr.Getdetails(data);
        }


        [Route("editdata/{id}")]
        public SMSMasterApprovalDTO editdata(int id)
        {
            SMSMasterApprovalDTO data = new SMSMasterApprovalDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.SMA_Id = id;
            //data.MI_Id = 5;
            return crStr.editdata(data);
        }
        [HttpPost]
        [Route("deactivate")]
        public SMSMasterApprovalDTO deactivate([FromBody] SMSMasterApprovalDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.deactivate(data);

        }

        [HttpPost]
        [Route("GetAttendence")]
        public SMSMasterApprovalDTO GetAttendence([FromBody] SMSMasterApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.MI_Id = 5;
            return crStr.GetAttendence(data);

        }
        [HttpPost]
        [Route("savedetails")]
        public SMSMasterApprovalDTO savedetails([FromBody] SMSMasterApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            // data.MI_Id = 5;
            return crStr.savedetails(data);

        }
    }

}
