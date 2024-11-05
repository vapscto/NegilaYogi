using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using corewebapi18072016.Delegates;
using DomainModel.Model.com.vapstech.Fee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    public class FeeActivityRequestController : Controller
    {
        FeeActivityRequestDelegate od = new FeeActivityRequestDelegate();
        // GET: api/<controller>

        [Route("loaddata")]
        public Adm_Master_ActivitiesDTO getinitialdropdowns([FromBody] Adm_Master_ActivitiesDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            //data.AMST_Id = 199508359;

            return od.getdata(data);
        }
        [Route("savedata")]
        public Adm_Master_ActivitiesDTO savedata([FromBody] Adm_Master_ActivitiesDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            //data.AMST_Id = 199508359;

            return od.savedata(data);
        }

        [Route("deletedata")]
        public Adm_Master_ActivitiesDTO deletedata([FromBody] Adm_Master_ActivitiesDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            //data.AMST_Id = 199508359;

            return od.deletedata(data);
        }
        [Route("loadapproval")]
        public Adm_Master_ActivitiesDTO loadapproval([FromBody] Adm_Master_ActivitiesDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.AMST_Id = 199508359;

            return od.loadapproval(data);
        }

        [Route("viewacaclslst")]
        public Adm_Master_ActivitiesDTO viewacaclslst([FromBody] Adm_Master_ActivitiesDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.AMST_Id = 199508359;

            return od.viewacaclslst(data);
        }

        [Route("viewstudentlist")]
        public Adm_Master_ActivitiesDTO viewstudentlist([FromBody] Adm_Master_ActivitiesDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.AMST_Id = 199508359;

            return od.viewstudentlist(data);
        }

        [Route("saveGroupdata")]
        public Adm_Master_ActivitiesDTO saveGroupdata([FromBody] Adm_Master_ActivitiesDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return od.saveGroupdata(data);
        }

        [Route("searching")]
        public Adm_Master_ActivitiesDTO searching([FromBody] Adm_Master_ActivitiesDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return od.searching(data);
        }

    }
}
