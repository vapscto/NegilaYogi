using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NaacServiceHub.com.vaps.LessonPlanner.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam.LessonPlanner;
using PreadmissionDTOs.NAAC.LessonPlanner;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.com.vaps.LessonPlanner.FacadeController
{
    [Route("api/[controller]")]
    public class MasterSchoolTopicFacadeController : Controller
    {
        public MasterSchoolTopicInterface _interface;
        public MasterSchoolTopicFacadeController(MasterSchoolTopicInterface _interfa)
        {
            _interface = _interfa;
        }
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
        [Route("Getdetails")]
        public MasterSchoolTopicDTO Getdetails([FromBody] MasterSchoolTopicDTO id)
        {
            return _interface.Getdetails(id);
        }
        [Route("savedetails")]
        public MasterSchoolTopicDTO savedetails([FromBody]   MasterSchoolTopicDTO data)
        {
            return _interface.savedetails(data);
        }
        [Route("editdeatils")]
        public MasterSchoolTopicDTO editdeatils([FromBody]   MasterSchoolTopicDTO data)
        {
            return _interface.editdeatils(data);
        }
        [Route("deactivate")]
        public MasterSchoolTopicDTO deactivate([FromBody]   MasterSchoolTopicDTO data)
        {
            return _interface.deactivate(data);
        }
        [Route("gettopicdetails")]
        public MasterSchoolTopicDTO gettopicdetails([FromBody]   MasterSchoolTopicDTO data)
        {
            return _interface.gettopicdetails(data);
        }
        
        [Route("validateordernumber")]
        public MasterSchoolTopicDTO validateordernumber([FromBody]   MasterSchoolTopicDTO data)
        {
            return _interface.validateordernumber(data);
        }
        [Route("onchangeyear")]
        public MasterSchoolTopicDTO onchangeyear([FromBody]   MasterSchoolTopicDTO data)
        {
            return _interface.onchangeyear(data);
        }
        [Route("onchangeclass")]
        public MasterSchoolTopicDTO onchangeclass([FromBody]   MasterSchoolTopicDTO data)
        {
            return _interface.onchangeclass(data);
        }

        // College
        [Route("getcollegedetails")]
        public LP_Master_MainTopic_CollegeDTO getcollegedetails([FromBody]   LP_Master_MainTopic_CollegeDTO data)
        {
            return _interface.getcollegedetails(data);
        }
        [Route("onchangecollegeyear")]
        public LP_Master_MainTopic_CollegeDTO onchangecollegeyear([FromBody] LP_Master_MainTopic_CollegeDTO data)
        {         
            return _interface.onchangecollegeyear(data);
        }
        [Route("onchangecourse")]
        public LP_Master_MainTopic_CollegeDTO onchangecourse([FromBody] LP_Master_MainTopic_CollegeDTO data)
        {
            return _interface.onchangecourse(data);
        }
        [Route("onchangebranch")]
        public LP_Master_MainTopic_CollegeDTO onchangebranch([FromBody] LP_Master_MainTopic_CollegeDTO data)
        {
            return _interface.onchangebranch(data);
        }
        [Route("onchangesemester")]
        public LP_Master_MainTopic_CollegeDTO onchangesemester([FromBody] LP_Master_MainTopic_CollegeDTO data)
        {
            return _interface.onchangesemester(data);
        }
        [Route("savecollegedetails")]
        public LP_Master_MainTopic_CollegeDTO savecollegedetails([FromBody] LP_Master_MainTopic_CollegeDTO data)
        {
            return _interface.savecollegedetails(data);
        }
        [Route("editcollegedeatils")]
        public LP_Master_MainTopic_CollegeDTO editcollegedeatils([FromBody] LP_Master_MainTopic_CollegeDTO data)
        {
            return _interface.editcollegedeatils(data);
        }

        [Route("collegedeactivate")]
        public LP_Master_MainTopic_CollegeDTO collegedeactivate([FromBody] LP_Master_MainTopic_CollegeDTO data)
        {
            return _interface.collegedeactivate(data);
        }
        [Route("getcollegetopicdetails")]
        public LP_Master_MainTopic_CollegeDTO getcollegetopicdetails([FromBody] LP_Master_MainTopic_CollegeDTO data)
        {
            return _interface.getcollegetopicdetails(data);
        }
        [Route("validatecollegeordernumber")]
        public LP_Master_MainTopic_CollegeDTO validatecollegeordernumber([FromBody] LP_Master_MainTopic_CollegeDTO data)
        {
            return _interface.validatecollegeordernumber(data);
        }

    }
}
