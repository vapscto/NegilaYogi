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
    public class SchoolSubjectWithMasterTopicMappingFacadeController : Controller
    {
        public SchoolSubjectWithMasterTopicMappingInterface _interface;
        public SchoolSubjectWithMasterTopicMappingFacadeController(SchoolSubjectWithMasterTopicMappingInterface _interfa)
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
        [Route("Getdetails")]
        public SchoolSubjectWithMasterTopicMappingDTO Getdetails([FromBody] SchoolSubjectWithMasterTopicMappingDTO id)
        {
            return _interface.Getdetails(id);
        }
        [Route("onchangesubject")]
        public SchoolSubjectWithMasterTopicMappingDTO onchangesubject([FromBody] SchoolSubjectWithMasterTopicMappingDTO id)
        {
            return _interface.onchangesubject(id);
        }

        [Route("onchangeunit")]
        public SchoolSubjectWithMasterTopicMappingDTO onchangeunit([FromBody] SchoolSubjectWithMasterTopicMappingDTO id)
        {
            return _interface.onchangeunit(id);
        }

        [Route("savedetails")]
        public SchoolSubjectWithMasterTopicMappingDTO savedetails([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _interface.savedetails(data);
        }

        [Route("deactivate")]
        public SchoolSubjectWithMasterTopicMappingDTO deactivate([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _interface.deactivate(data);
        }
        [Route("validateordernumber")]
        public SchoolSubjectWithMasterTopicMappingDTO validateordernumber([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _interface.validateordernumber(data);
        }
        [Route("editdeatils")]
        public SchoolSubjectWithMasterTopicMappingDTO editdeatils([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _interface.editdeatils(data);
        }
        [Route("onselecttopic")]
        public SchoolSubjectWithMasterTopicMappingDTO onselecttopic([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _interface.onselecttopic(data);
        }
        [Route("viewuploadflies")]
        public SchoolSubjectWithMasterTopicMappingDTO viewuploadflies([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _interface.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public SchoolSubjectWithMasterTopicMappingDTO deleteuploadfile([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _interface.deleteuploadfile(data);
        }

        // Topic Resource Mapping

        [Route("Getdetailsmapping")]
        public SchoolSubjectWithMasterTopicMappingDTO Getdetailsmapping([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _interface.Getdetailsmapping(data);
        }

        [Route("onchangetopic")]
        public SchoolSubjectWithMasterTopicMappingDTO onchangetopic([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _interface.onchangetopic(data);
        }
        [Route("onchangesubtopic")]
        public SchoolSubjectWithMasterTopicMappingDTO onchangesubtopic([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _interface.onchangesubtopic(data);
        }
        [Route("savemapping")]
        public SchoolSubjectWithMasterTopicMappingDTO savemapping([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _interface.savemapping(data);
        }

        [Route("onchangeyear")]
        public SchoolSubjectWithMasterTopicMappingDTO onchangeyear([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _interface.onchangeyear(data);
        }
        [Route("onchangeclass")]
        public SchoolSubjectWithMasterTopicMappingDTO onchangeclass([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _interface.onchangeclass(data);
        }
        // College 
        [Route("Getcollegedetails")]
        public CollegeSubjTopicMappingDTO Getcollegedetails([FromBody]  CollegeSubjTopicMappingDTO data)
        {
            return _interface.Getcollegedetails(data);
        }
        [Route("collegeonchangeyear")]
        public CollegeSubjTopicMappingDTO collegeonchangeyear([FromBody]  CollegeSubjTopicMappingDTO data)
        {
            return _interface.collegeonchangeyear(data);
        }
        [Route("collegeonchangecourse")]
        public CollegeSubjTopicMappingDTO collegeonchangecourse([FromBody]  CollegeSubjTopicMappingDTO data)
        {
            return _interface.collegeonchangecourse(data);
        }
        [Route("collegeonchangebranch")]
        public CollegeSubjTopicMappingDTO collegeonchangebranch([FromBody]  CollegeSubjTopicMappingDTO data)
        {
            return _interface.collegeonchangebranch(data);
        }
        [Route("collegeonchangesemester")]
        public CollegeSubjTopicMappingDTO collegeonchangesemester([FromBody]  CollegeSubjTopicMappingDTO data)
        {
            return _interface.collegeonchangesemester(data);
        }
        [Route("onchangecollegesubject")]
        public CollegeSubjTopicMappingDTO onchangecollegesubject([FromBody]  CollegeSubjTopicMappingDTO data)
        {
            return _interface.onchangecollegesubject(data);
        }
        [Route("onchangecollegeunit")]
        public CollegeSubjTopicMappingDTO onchangecollegeunit([FromBody]  CollegeSubjTopicMappingDTO data)
        {
            return _interface.onchangecollegeunit(data);
        }
        [Route("savecollegedetails")]
        public CollegeSubjTopicMappingDTO savecollegedetails([FromBody]  CollegeSubjTopicMappingDTO data)
        {
            return _interface.savecollegedetails(data);
        }
        [Route("editcollegedeatils")]
        public CollegeSubjTopicMappingDTO editcollegedeatils([FromBody]  CollegeSubjTopicMappingDTO data)
        {
            return _interface.editcollegedeatils(data);
        }
        [Route("collegedeactivate")]
        public CollegeSubjTopicMappingDTO collegedeactivate([FromBody]  CollegeSubjTopicMappingDTO data)
        {
            return _interface.collegedeactivate(data);
        }
        [Route("oncollegeselecttopic")]
        public CollegeSubjTopicMappingDTO oncollegeselecttopic([FromBody]  CollegeSubjTopicMappingDTO data)
        {
            return _interface.oncollegeselecttopic(data);
        }
        [Route("viewcollegeuploadflies")]
        public CollegeSubjTopicMappingDTO viewcollegeuploadflies([FromBody]  CollegeSubjTopicMappingDTO data)
        {
            return _interface.viewcollegeuploadflies(data);
        }
        [Route("deletecollegeuploadfile")]
        public CollegeSubjTopicMappingDTO deletecollegeuploadfile([FromBody]  CollegeSubjTopicMappingDTO data)
        {
            return _interface.deletecollegeuploadfile(data);
        }
        [Route("validatecollegeordernumber")]
        public CollegeSubjTopicMappingDTO validatecollegeordernumber([FromBody]  CollegeSubjTopicMappingDTO data)
        {
            return _interface.validatecollegeordernumber(data);
        }
    }
}
