using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Medical;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Medical;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Medical
{
    [Route("api/[controller]")]
    public class NAAC_MC_VACcommonController : Controller
    {


        public NAAC_MC_VACcommonDelegate _objdel = new NAAC_MC_VACcommonDelegate();

       
        [Route("loaddata/{id:int}")]
        public NAAC_MC_VACcommon_DTO loaddata(int id)
        {
            NAAC_MC_VACcommon_DTO data = new NAAC_MC_VACcommon_DTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.loaddata(data);
        }

        [Route("savedata141")]
        public NAAC_MC_VACcommon_DTO savedata141([FromBody]NAAC_MC_VACcommon_DTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savedata141(data);
        }

        [Route("editdata141")]
        public NAAC_MC_VACcommon_DTO editdata141([FromBody]NAAC_MC_VACcommon_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.editdata141(data);
        }

        [Route("savedata142")]
        public NAAC_MC_VACcommon_DTO savedata142([FromBody]NAAC_MC_VACcommon_DTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savedata142(data);
        }
       
        [Route("M_savedata221")]
        public NAAC_MC_VACcommon_DTO M_savedata221([FromBody]NAAC_MC_VACcommon_DTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.M_savedata221(data);
        }

        [Route("M_savedata232")]
        public NAAC_MC_VACcommon_DTO M_savedata232([FromBody]NAAC_MC_VACcommon_DTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.M_savedata232(data);
        }

        [Route("M_savedata254")]
        public NAAC_MC_VACcommon_DTO M_savedata254([FromBody]NAAC_MC_VACcommon_DTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.M_savedata254(data);
        }
        
    }
}
