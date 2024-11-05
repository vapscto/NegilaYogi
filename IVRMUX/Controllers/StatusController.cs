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
    public class StatusController : Controller
    {
        private static FacadeUrl _config;
        CommonDelegate<Controller, Controller> sad = new CommonDelegate<Controller, Controller>();
        private FacadeUrl fdu = new FacadeUrl();

        //public StatusController(IOptions<FacadeUrl> settings)
        //{
        //    _config = settings.Value;
        //    new StatusDelegate(_config);
        //    fdu = _config;
        //}

        // get initial dropdown data
        [Route("getinitialdata")]
        public CommonDTO getInitialData()
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //return sad.getInitailData(mi_id);
            CommonDelegate<CommonDTO, CommonDTO> sad1 = new CommonDelegate<CommonDTO, CommonDTO>();
            var aa = sad1.GetDataById(mi_id, "StatusFacade/getinitialdata/");
            CommonDTO cdto = (CommonDTO)aa;
            return cdto;
        }

        // search student based on year, class and status

        [Route("SearchData")]
        public CommonDTO getStudentOnSearchFilter([FromBody] CommonDTO cdto)
        {
            //return sad.getStudentOnSearchFilter(cdto);
            CommonDelegate<CommonDTO, CommonDTO> sad1 = new CommonDelegate<CommonDTO, CommonDTO>();
            cdto.IVRM_MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            var aa = sad1.POSTData(cdto, "StatusFacade/getdataonsearchfilter/");
            CommonDTO stu = (CommonDTO)aa;
            return stu;
        }


        // post data i.e changed status of student
        [HttpPost]
        public CommonDTO saveData([FromBody] CommonDTO studentdata)
        {
            //return sad.saveData(studentdata);
         
            studentdata.mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            studentdata.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            CommonDelegate<CommonDTO, CommonDTO> sad1 = new CommonDelegate<CommonDTO, CommonDTO>();
            var aa = sad1.POSTData(studentdata, "StatusFacade/savedata/");
            CommonDTO stu = (CommonDTO)aa;
            return stu;
        }
    }
}