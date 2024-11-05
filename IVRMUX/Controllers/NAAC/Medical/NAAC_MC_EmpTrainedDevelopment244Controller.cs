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
    public class NAAC_MC_EmpTrainedDevelopment244Controller : Controller
    {
        NAAC_MC_EmpTrainedDevelopment244Delegate _objdel = new NAAC_MC_EmpTrainedDevelopment244Delegate();
        [Route("loaddata/{id:int}")]
        public NAAC_MC_EmpTrainedDevelopment244_DTO loaddata(int id)
        {
            NAAC_MC_EmpTrainedDevelopment244_DTO data = new NAAC_MC_EmpTrainedDevelopment244_DTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = id;
            return _objdel.loaddata(data);
        }
        [Route("save")]
        public NAAC_MC_EmpTrainedDevelopment244_DTO save([FromBody]NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.save(data);
        }
        [Route("deactive")]
        public NAAC_MC_EmpTrainedDevelopment244_DTO deactive([FromBody] NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.deactive(data);
        }
        [Route("EditData")]
        public NAAC_MC_EmpTrainedDevelopment244_DTO EditData([FromBody] NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.EditData(data);
        }



        [Route("viewuploadflies")]
        public NAAC_MC_EmpTrainedDevelopment244_DTO viewuploadflies([FromBody] NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.viewuploadflies(data);
        }


        [Route("deleteuploadfile")]
        public NAAC_MC_EmpTrainedDevelopment244_DTO deleteuploadfile([FromBody] NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.deleteuploadfile(data);
        }


        [Route("savemedicaldatawisecomments")]
        public NAAC_MC_EmpTrainedDevelopment244_DTO savemedicaldatawisecomments([FromBody] NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savemedicaldatawisecomments(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_MC_EmpTrainedDevelopment244_DTO savefilewisecomments([FromBody] NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savefilewisecomments(data);
        }
        [Route("getcomment")]
        public NAAC_MC_EmpTrainedDevelopment244_DTO getcomment([FromBody] NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getcomment(data);
        }
        [Route("getfilecomment")]
        public NAAC_MC_EmpTrainedDevelopment244_DTO getfilecomment([FromBody] NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getfilecomment(data);
        }

    }
}
