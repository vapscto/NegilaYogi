using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Exam.LessonPlanner;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam.LessonPlanner;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Exam.LessonPlanner
{
    [Route("api/[controller]")]
    public class SchoolMasterUnitController : Controller
    {

        public SchoolMasterUnitDelegate _delg = new SchoolMasterUnitDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("Getdetails/{id:int}")]
        public SchoolMasterUnitDTO Getdetails(int id)
        {
            SchoolMasterUnitDTO data = new SchoolMasterUnitDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMU_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.Getdetails(data);
        }
        [Route("savedetails")]
        public SchoolMasterUnitDTO savedetails([FromBody] SchoolMasterUnitDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMU_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.savedetails(data);
        }
        [Route("editdeatils")]
        public SchoolMasterUnitDTO editdeatils([FromBody] SchoolMasterUnitDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMU_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.editdeatils(data);
        }
        [Route("deactivate")]
        public SchoolMasterUnitDTO deactivate([FromBody] SchoolMasterUnitDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMU_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.deactivate(data);
        }
        [Route("validateordernumber")]
        public SchoolMasterUnitDTO validateordernumber([FromBody] SchoolMasterUnitDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMU_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.validateordernumber(data);
        }

        // Master Unit Topic Mapping
        [Route("Getdetailsmapping/{id:int}")]
        public SchoolMasterUnitDTO Getdetailsmapping(int id)
        {
            SchoolMasterUnitDTO data = new SchoolMasterUnitDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMU_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.Getdetailsmapping(data);
        }
        [Route("gettopicnames")]
        public SchoolMasterUnitDTO gettopicnames([FromBody] SchoolMasterUnitDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMU_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.gettopicnames(data);
        }
        [Route("savemappingdetails")]
        public SchoolMasterUnitDTO savemappingdetails([FromBody] SchoolMasterUnitDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMU_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.savemappingdetails(data);
        }
        [Route("deactivatemapping")]
        public SchoolMasterUnitDTO deactivatemapping([FromBody] SchoolMasterUnitDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMU_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.deactivatemapping(data);
        }
        
    }
}
