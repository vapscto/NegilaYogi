using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using CommonLibrary;


namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class countstatusreportController : Controller
    {
        countstatusreportDelegate countstatusreportDelegate = new countstatusreportDelegate();
        private static FacadeUrl _config;
        CommonDelegate<Controller, Controller> sad = new CommonDelegate<Controller, Controller>();
        private FacadeUrl fdu = new FacadeUrl();
       
        // get initial dropdown data
        [Route("getinitialdata/")]
        public CommonDTO getInitialData()
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //return sad.getInitailData(mi_id);
            CommonDelegate<CommonDTO, CommonDTO> sad1 = new CommonDelegate<CommonDTO, CommonDTO>();
            var aa = sad1.GetDataById(mi_id, "countstatusreportFacade/getinitialdata/");
            CommonDTO cdto = (CommonDTO)aa;
            return cdto;
        }

       

        [HttpPost]
        [Route("Getdetails/")]
        public CommonDTO Getdetails([FromBody] CommonDTO MMD)
        {
            MMD.IVRM_MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return countstatusreportDelegate.GetData(MMD);
        }

    }
}