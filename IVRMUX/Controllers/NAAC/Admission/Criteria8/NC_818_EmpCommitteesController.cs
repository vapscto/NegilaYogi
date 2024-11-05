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
    public class NC_818_EmpCommitteesController : Controller
    {
        public NC_818_EmpCommitteesDelegate obj = new NC_818_EmpCommitteesDelegate();
        // GET: api/<controller>
        [Route("loaddata/{id:int}")]
        public NC_818_EmpCommitteesDTO loaddata(int id)
        {
            NC_818_EmpCommitteesDTO data = new NC_818_EmpCommitteesDTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return obj.loaddata(data);
        }
        [Route("savedata")]
        public NC_818_EmpCommitteesDTO savedata([FromBody] NC_818_EmpCommitteesDTO data)
        {

            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return obj.savedata(data);
        }
        [Route("editdata")]
        public NC_818_EmpCommitteesDTO editdata([FromBody] NC_818_EmpCommitteesDTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return obj.editdata(data);
        }
        [Route("deactivY")]
        public NC_818_EmpCommitteesDTO deactivY([FromBody] NC_818_EmpCommitteesDTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return obj.deactivY(data);
        }
        [Route("viewuploadflies")]
        public NC_818_EmpCommitteesDTO viewuploadflies([FromBody] NC_818_EmpCommitteesDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public NC_818_EmpCommitteesDTO deleteuploadfile([FromBody] NC_818_EmpCommitteesDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.deleteuploadfile(data);
        }
        [Route("getcomment")]
        public NC_818_EmpCommitteesDTO getcomment([FromBody] NC_818_EmpCommitteesDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.getcomment(data);
        }
        [Route("getfilecomment")]
        public NC_818_EmpCommitteesDTO getfilecomment([FromBody] NC_818_EmpCommitteesDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.getfilecomment(data);
        }
        [Route("savecomments")]
        public NC_818_EmpCommitteesDTO savecomments([FromBody] NC_818_EmpCommitteesDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.savecomments(data);
        }
        [Route("savefilewisecomments")]
        public NC_818_EmpCommitteesDTO savefilewisecomments([FromBody] NC_818_EmpCommitteesDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return obj.savefilewisecomments(data);
        }
    }
}
