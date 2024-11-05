
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
//using DomainModel.Model.com.vapstech.p;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class PrincipalDashboardController : Controller
    {


        PrincipalDashboardDelegates prStr = new PrincipalDashboardDelegates();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails")]
        public PrincipalDashboardDTO Getdetails(PrincipalDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id= Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.PaymentNootificationPrinicipal = Convert.ToInt64(HttpContext.Session.GetInt32("PaymentNootificationPrinicipal"));
            return prStr.Getdetails(data);            
        }
        [Route("GetDataByYear/{id:int}")]
        public PrincipalDashboardDTO GetDataByYear(int id)
        {
            PrincipalDashboardDTO data = new PrincipalDashboardDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = id;
            return prStr.Getdetails(data);
        }

        [Route("onclick_notice")]
        public PrincipalDashboardDTO onclick_notice([FromBody]PrincipalDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            if (data.OnClickOrOnChange == "OnClick")
            {
                data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            //sddto.ASMCL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMCL_Id"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            //data.Feecheck = Convert.ToInt32(HttpContext.Session.GetInt32("Feecheck"));
            //data.stdupdate = Convert.ToInt32(HttpContext.Session.GetInt32("StudentUpdateRequest"));
            //HttpContext.Session.SetInt32("Feecheck", 1);
            //HttpContext.Session.SetInt32("StudentUpdateRequest", 1);
            return prStr.onclick_notice(data);
        }

        [Route("viewnotice")]
        public PrincipalDashboardDTO viewnotice([FromBody]PrincipalDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return prStr.viewnotice(data);
        }

    }

}
