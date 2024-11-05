using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Admission.Criteria8;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Admission.Criteria8;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Admission.Criteria8
{
    [Route("api/[controller]")]
    public class NAAC_811MC_NEETController : Controller
    {
        public NAAC_811MC_NEETDelegate obj = new NAAC_811MC_NEETDelegate();
        // GET: api/<controller>
        [Route("loaddata/{id:int}")]
        public NAAC_811MC_NEET_DTO loaddata(int id)
        {
            NAAC_811MC_NEET_DTO data = new NAAC_811MC_NEET_DTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return obj.loaddata(data);
        }
        [Route("savedata")]
        public NAAC_811MC_NEET_DTO savedata([FromBody] NAAC_811MC_NEET_DTO data)
        {
           
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return obj.savedata(data);
        }
        [Route("editdata")]
        public NAAC_811MC_NEET_DTO editdata([FromBody] NAAC_811MC_NEET_DTO data)
        {
           
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return obj.editdata(data);
        }
        [Route("deactivY")]
        public NAAC_811MC_NEET_DTO deactivY([FromBody] NAAC_811MC_NEET_DTO data)
        {
           
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return obj.deactivY(data);
        }
        [Route("viewuploadflies")]
        public NAAC_811MC_NEET_DTO viewuploadflies([FromBody] NAAC_811MC_NEET_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public NAAC_811MC_NEET_DTO deleteuploadfile([FromBody] NAAC_811MC_NEET_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.deleteuploadfile(data);
        }
        [Route("getcomment")]
        public NAAC_811MC_NEET_DTO getcomment([FromBody] NAAC_811MC_NEET_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.getcomment(data);
        }
        [Route("getfilecomment")]
        public NAAC_811MC_NEET_DTO getfilecomment([FromBody] NAAC_811MC_NEET_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.getfilecomment(data);
        }
        [Route("savecomments")]
        public NAAC_811MC_NEET_DTO savecomments([FromBody] NAAC_811MC_NEET_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.savecomments(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_811MC_NEET_DTO savefilewisecomments([FromBody] NAAC_811MC_NEET_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.savefilewisecomments(data);
        }
    }
}

