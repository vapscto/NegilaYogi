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
    public class MasterSchoolTopicController : Controller
    {

        public MasterSchoolTopicDelegate _delg = new MasterSchoolTopicDelegate();

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

        // School
        [Route("Getdetails/{id:int}")]
        public MasterSchoolTopicDTO Getdetails(int id)
        {
            MasterSchoolTopicDTO data = new MasterSchoolTopicDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.Getdetails(data);
        }
        [Route("savedetails")]
        public MasterSchoolTopicDTO savedetails([FromBody]   MasterSchoolTopicDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.savedetails(data);
        }
        [Route("editdeatils")]
        public MasterSchoolTopicDTO editdeatils([FromBody]   MasterSchoolTopicDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.editdeatils(data);
        }
        [Route("deactivate")]
        public MasterSchoolTopicDTO deactivate([FromBody]   MasterSchoolTopicDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.deactivate(data);
        }
        [Route("gettopicdetails")]
        public MasterSchoolTopicDTO gettopicdetails([FromBody]   MasterSchoolTopicDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.gettopicdetails(data);
        }

        [Route("validateordernumber")]
        public MasterSchoolTopicDTO validateordernumber([FromBody]   MasterSchoolTopicDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.validateordernumber(data);
        }

        [Route("onchangeyear")]
        public MasterSchoolTopicDTO onchangeyear([FromBody]   MasterSchoolTopicDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangeyear(data);
        }
        [Route("onchangeclass")]
        public MasterSchoolTopicDTO onchangeclass([FromBody]   MasterSchoolTopicDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangeclass(data);
        }

        // College 
        [Route("getcollegedetails/{id:int}")]
        public LP_Master_MainTopic_CollegeDTO getcollegedetails(int id)
        {
            LP_Master_MainTopic_CollegeDTO data = new LP_Master_MainTopic_CollegeDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getcollegedetails(data);
        }
        [Route("onchangecollegeyear")]
        public LP_Master_MainTopic_CollegeDTO onchangecollegeyear([FromBody] LP_Master_MainTopic_CollegeDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMMTC_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangecollegeyear(data);
        }
        [Route("onchangecourse")]
        public LP_Master_MainTopic_CollegeDTO onchangecourse([FromBody] LP_Master_MainTopic_CollegeDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMMTC_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangecourse(data);
        }
        [Route("onchangebranch")]
        public LP_Master_MainTopic_CollegeDTO onchangebranch([FromBody] LP_Master_MainTopic_CollegeDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMMTC_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangebranch(data);
        }
        [Route("onchangesemester")]
        public LP_Master_MainTopic_CollegeDTO onchangesemester([FromBody] LP_Master_MainTopic_CollegeDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMMTC_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangesemester(data);
        }
        [Route("savecollegedetails")]
        public LP_Master_MainTopic_CollegeDTO savecollegedetails([FromBody] LP_Master_MainTopic_CollegeDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMMTC_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.savecollegedetails(data);
        }
        [Route("editcollegedeatils")]
        public LP_Master_MainTopic_CollegeDTO editcollegedeatils([FromBody] LP_Master_MainTopic_CollegeDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMMTC_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.editcollegedeatils(data);
        }

        [Route("collegedeactivate")]
        public LP_Master_MainTopic_CollegeDTO collegedeactivate([FromBody] LP_Master_MainTopic_CollegeDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMMTC_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.collegedeactivate(data);
        }
        [Route("getcollegetopicdetails")]
        public LP_Master_MainTopic_CollegeDTO getcollegetopicdetails([FromBody] LP_Master_MainTopic_CollegeDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMMTC_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getcollegetopicdetails(data);
        }
        [Route("validatecollegeordernumber")]
        public LP_Master_MainTopic_CollegeDTO validatecollegeordernumber([FromBody] LP_Master_MainTopic_CollegeDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LPMMTC_CreatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.validatecollegeordernumber(data);
        }
    }
}
