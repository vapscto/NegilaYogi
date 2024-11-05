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
    public class DC_8111_ExpenditureController : Controller
    {
        public DC_8111_ExpenditureDelegate obj = new DC_8111_ExpenditureDelegate();
        // GET: api/<controller>
        [Route("loaddata/{id:int}")]
        public DC_8111_ExpenditureDTO loaddata(int id)
        {
            DC_8111_ExpenditureDTO data = new DC_8111_ExpenditureDTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return obj.loaddata(data);
        }
        [Route("savedata")]
        public DC_8111_ExpenditureDTO savedata([FromBody] DC_8111_ExpenditureDTO data)
        {

            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return obj.savedata(data);
        }
        [Route("editdata")]
        public DC_8111_ExpenditureDTO editdata([FromBody] DC_8111_ExpenditureDTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return obj.editdata(data);
        }
        [Route("deactivY")]
        public DC_8111_ExpenditureDTO deactivY([FromBody] DC_8111_ExpenditureDTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return obj.deactivY(data);
        }
        [Route("viewuploadflies")]
        public DC_8111_ExpenditureDTO viewuploadflies([FromBody] DC_8111_ExpenditureDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public DC_8111_ExpenditureDTO deleteuploadfile([FromBody] DC_8111_ExpenditureDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.deleteuploadfile(data);
        }
        [Route("getcomment")]
        public DC_8111_ExpenditureDTO getcomment([FromBody] DC_8111_ExpenditureDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.getcomment(data);
        }
        [Route("getfilecomment")]
        public DC_8111_ExpenditureDTO getfilecomment([FromBody] DC_8111_ExpenditureDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.getfilecomment(data);
        }
        [Route("savecomments")]
        public DC_8111_ExpenditureDTO savecomments([FromBody] DC_8111_ExpenditureDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.savecomments(data);
        }
        [Route("savefilewisecomments")]
        public DC_8111_ExpenditureDTO savefilewisecomments([FromBody] DC_8111_ExpenditureDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.savefilewisecomments(data);
        }
    }
}
