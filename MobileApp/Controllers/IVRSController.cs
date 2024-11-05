using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MobileApp.Delegates;
using PreadmissionDTOs.com.vaps.Portals.IVRS;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MobileApp.Controllers
{
    [Route("api/[controller]")]
    public class IVRSController : Controller
    {
        IVRSDelegates pre = new IVRSDelegates();
        [Route("IvrDetails")]
        public JsonResult getdata(long MI_ID, int IIVRSC_SchoolFlg, [FromQuery]string operation, [FromQuery]string IVRA_TPIN,string exeId )
        {
            IVRSDTO data = new IVRSDTO();
            data.operation=operation;
            data.IVRA_TPIN = IVRA_TPIN;
            data.exeId = exeId;
            data.MI_Id = MI_ID;
            data.IIVRSC_SchoolFlg = IIVRSC_SchoolFlg;
            var res= pre.getdata(data);
            return Json(res.Value);            
        }

        [Route("IvrBranch")]
        public JsonResult getbranch(long vno)
        {
            IVRSDTO data = new IVRSDTO();         
            data.vno = vno;
            var res = pre.getbranch(data);
            return Json(res.Value);
        }
        [HttpPost]
        [Route("updatecredits")]
        public JsonResult updatecredits([FromBody]IVRSDTO data)
        {
            var res = pre.updatecredits(data);
            return Json(res.Value);
        }
        [HttpPost]
        [Route("UpdateMobile")]
        public JsonResult UpdateMobile([FromBody]IVRSDTO data)
        {
            var res = pre.UpdateMobile(data);
            return Json(res.Value);
        }
       
    }
}
