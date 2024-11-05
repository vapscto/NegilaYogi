using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Exam.LessonPlanner;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam.LessonPlanner;
using PreadmissionDTOs.NAAC.LessonPlanner;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Exam.LessonPlanner
{
    [Route("api/[controller]")]
    public class SchoolSubjectWithMasterTopicMappingController : Controller
    {
        public SchoolSubjectWithMasterTopicMappingDelegate _delg = new SchoolSubjectWithMasterTopicMappingDelegate();
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
        public SchoolSubjectWithMasterTopicMappingDTO Getdetails(int id)
        {
            SchoolSubjectWithMasterTopicMappingDTO data = new SchoolSubjectWithMasterTopicMappingDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.Getdetails(data);
        }

        [Route("savedetails")]
        public SchoolSubjectWithMasterTopicMappingDTO savedetails([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.savedetails(data);
        }

        [Route("onchangesubject")]
        public SchoolSubjectWithMasterTopicMappingDTO onchangesubject([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangesubject(data);
        }

        [Route("onchangeunit")]
        public SchoolSubjectWithMasterTopicMappingDTO onchangeunit([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangeunit(data);
        }

        [Route("viewuploadflies")]
        public SchoolSubjectWithMasterTopicMappingDTO viewuploadflies([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.viewuploadflies(data);
        }

        [Route("deleteuploadfile")]
        public SchoolSubjectWithMasterTopicMappingDTO deleteuploadfile([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.deleteuploadfile(data);
        }

        [Route("deactivate")]
        public SchoolSubjectWithMasterTopicMappingDTO deactivate([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.deactivate(data);
        }

        [Route("editdeatils")]
        public SchoolSubjectWithMasterTopicMappingDTO editdeatils([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.editdeatils(data);
        }

        [Route("validateordernumber")]
        public SchoolSubjectWithMasterTopicMappingDTO validateordernumber([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.validateordernumber(data);
        }

        [Route("onselecttopic")]
        public SchoolSubjectWithMasterTopicMappingDTO onselecttopic([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.onselecttopic(data);
        }

        // Lopic Resource Mapping

        [Route("Getdetailsmapping/{id:int}")]
        public SchoolSubjectWithMasterTopicMappingDTO Getdetailsmapping(int id)
        {
            SchoolSubjectWithMasterTopicMappingDTO data = new SchoolSubjectWithMasterTopicMappingDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.Getdetailsmapping(data);
        }
        [Route("onchangetopic")]
        public SchoolSubjectWithMasterTopicMappingDTO onchangetopic([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangetopic(data);
        }
        [Route("onchangesubtopic")]
        public SchoolSubjectWithMasterTopicMappingDTO onchangesubtopic([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangesubtopic(data);
        }

        [Route("savemapping")]
        public SchoolSubjectWithMasterTopicMappingDTO savemapping([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.savemapping(data);
        }

        [Route("onchangeyear")]
        public SchoolSubjectWithMasterTopicMappingDTO onchangeyear([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangeyear(data);
        }
        [Route("onchangeclass")]
        public SchoolSubjectWithMasterTopicMappingDTO onchangeclass([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangeclass(data);
        }

        // College
        [Route("Getcollegedetails/{id:int}")]
        public CollegeSubjTopicMappingDTO Getcollegedetails(int id)
        {
            CollegeSubjTopicMappingDTO data = new CollegeSubjTopicMappingDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMTC_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.Getcollegedetails(data);
        }

        [Route("collegeonchangeyear")]
        public CollegeSubjTopicMappingDTO collegeonchangeyear([FromBody] CollegeSubjTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMTC_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.collegeonchangeyear(data);
        }
        [Route("collegeonchangecourse")]
        public CollegeSubjTopicMappingDTO collegeonchangecourse([FromBody] CollegeSubjTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMTC_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.collegeonchangecourse(data);
        }
        [Route("collegeonchangebranch")]
        public CollegeSubjTopicMappingDTO collegeonchangebranch([FromBody] CollegeSubjTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMTC_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.collegeonchangebranch(data);
        }
        [Route("collegeonchangesemester")]
        public CollegeSubjTopicMappingDTO collegeonchangesemester([FromBody] CollegeSubjTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMTC_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.collegeonchangesemester(data);
        }

        [Route("onchangecollegesubject")]
        public CollegeSubjTopicMappingDTO onchangecollegesubject([FromBody] CollegeSubjTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMTC_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangecollegesubject(data);
        }
        [Route("onchangecollegeunit")]
        public CollegeSubjTopicMappingDTO onchangecollegeunit([FromBody] CollegeSubjTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMTC_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangecollegeunit(data);
        }
        [Route("savecollegedetails")]
        public CollegeSubjTopicMappingDTO savecollegedetails([FromBody] CollegeSubjTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMTC_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.savecollegedetails(data);
        }
        [Route("editcollegedeatils")]
        public CollegeSubjTopicMappingDTO editcollegedeatils([FromBody] CollegeSubjTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMTC_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.editcollegedeatils(data);
        }

        [Route("collegedeactivate")]
        public CollegeSubjTopicMappingDTO collegedeactivate([FromBody] CollegeSubjTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMTC_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.collegedeactivate(data);
        }

        [Route("oncollegeselecttopic")]
        public CollegeSubjTopicMappingDTO oncollegeselecttopic([FromBody] CollegeSubjTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMTC_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.oncollegeselecttopic(data);
        }

        [Route("viewcollegeuploadflies")]
        public CollegeSubjTopicMappingDTO viewcollegeuploadflies([FromBody] CollegeSubjTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMTC_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.viewcollegeuploadflies(data);
        }

        [Route("deletecollegeuploadfile")]
        public CollegeSubjTopicMappingDTO deletecollegeuploadfile([FromBody] CollegeSubjTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMTC_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.deletecollegeuploadfile(data);
        }
        [Route("validatecollegeordernumber")]
        public CollegeSubjTopicMappingDTO validatecollegeordernumber([FromBody] CollegeSubjTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMTC_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.validatecollegeordernumber(data);
        }

    }
}
