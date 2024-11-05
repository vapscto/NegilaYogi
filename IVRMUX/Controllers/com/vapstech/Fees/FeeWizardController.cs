using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Fees;
using IVRMUX.Delegates.com.vapstech.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
   
    [Route("api/[controller]")]
    public class FeeWizardController : Controller
    {
        FeeWizardDelegate FGD = new FeeWizardDelegate();



        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeWizardDTO Get([FromQuery] int id)
        {
            FeeWizardDTO data = new FeeWizardDTO();
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.MI_Id = id;
            return FGD.getdetails(data);
        }






        [HttpPost]
        [Route("savedetailY")]
        public FeeWizardDTO savedetailY([FromBody] FeeWizardDTO GrouppageY)
        {
            GrouppageY.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            GrouppageY.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return FGD.savedetailsY(GrouppageY);
        }

        [HttpPost]
        [Route("changacademicyear")]
        public FeeWizardDTO changacademicyear([FromBody] FeeWizardDTO GrouppageY)
        {
            GrouppageY.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            GrouppageY.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return FGD.changacademicyear(GrouppageY);
        }
        [HttpPost]
        [Route("deactivateY")]
        public FeeWizardDTO deactvateY([FromBody] FeeWizardDTO id)
        {
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            id.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return FGD.deactivateY(id);
        }
        [HttpPost]
        [Route("savedetailFGH")]
        public FeeWizardDTO savedetailFGH([FromBody] FeeWizardDTO GrouppageY)
        {
            GrouppageY.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            GrouppageY.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return FGD.savedetailFGH(GrouppageY);
        }

        [HttpDelete]
        [Route("Deletedetails/{id:int}")]
        public FeeWizardDTO Delete(int id)
        {
            return FGD.deleterec(id);
        }

        [HttpPost]
        [Route("savedetailYCC")]
        public FeeWizardDTO savedetailYCC([FromBody] FeeWizardDTO GrouppageY)
        {
            GrouppageY.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            GrouppageY.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return FGD.savedetailYCC(GrouppageY);
        }
        [HttpDelete]
        [Route("deletepagesY/{id:int}")]
        public FeeWizardDTO DeleteY(int id)
        {
            return FGD.deleterecY(id);
        }
        [HttpPost]
        [Route("savedetailFMA")]
        public FeeWizardDTO savedetailFMA([FromBody] FeeWizardDTO GrouppageY)
        {
            GrouppageY.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            GrouppageY.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return FGD.savedetailFMA(GrouppageY);
        }

        [Route("Deletedetails")]
        public FeeWizardDTO Delete([FromBody]FeeWizardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return FGD.deleterecFMA(data);
        }

        [HttpPost]
        [Route("savedetailFMAG")]
        public FeeWizardDTO savedetailFMAG([FromBody] FeeWizardDTO GrouppageY)
        {
            GrouppageY.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            GrouppageY.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return FGD.savedetailFMAG(GrouppageY);
        }
        [Route("delete")]
        public FeeWizardDTO deletee([FromBody] FeeWizardDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.user_id = UserId;

            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return FGD.deletedata(data);
        }

    }
}
