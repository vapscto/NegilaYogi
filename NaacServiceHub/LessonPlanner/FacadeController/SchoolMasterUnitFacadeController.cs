using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NaacServiceHub.com.vaps.LessonPlanner.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam.LessonPlanner;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.com.vaps.LessonPlanner.FacadeController
{
    [Route("api/[controller]")]
    public class SchoolMasterUnitFacadeController : Controller
    {
        public SchoolMasterUnitInterface _interface;

        public SchoolMasterUnitFacadeController(SchoolMasterUnitInterface _inter)
        {
            _interface = _inter;
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
        public SchoolMasterUnitDTO Getdetails([FromBody] SchoolMasterUnitDTO data)
        {
            return _interface.Getdetails(data);
        }
        [Route("savedetails")]
        public SchoolMasterUnitDTO savedetails([FromBody] SchoolMasterUnitDTO data)
        {
            return _interface.savedetails(data);
        }
        [Route("editdeatils")]
        public SchoolMasterUnitDTO editdeatils([FromBody] SchoolMasterUnitDTO data)
        {
            return _interface.editdeatils(data);
        }
        [Route("deactivate")]
        public SchoolMasterUnitDTO deactivate([FromBody] SchoolMasterUnitDTO data)
        {
            return _interface.deactivate(data);
        }
        [Route("validateordernumber")]
        public SchoolMasterUnitDTO validateordernumber([FromBody] SchoolMasterUnitDTO data)
        {
            return _interface.validateordernumber(data);
        }

        // Master Unit Topic Mapping
        [Route("Getdetailsmapping")]
        public SchoolMasterUnitDTO Getdetailsmapping([FromBody] SchoolMasterUnitDTO data)
        {
            return _interface.Getdetailsmapping(data);
        }
        [Route("gettopicnames")]
        public SchoolMasterUnitDTO gettopicnames([FromBody] SchoolMasterUnitDTO data)
        {
            return _interface.gettopicnames(data);
        }
        [Route("savemappingdetails")]
        public SchoolMasterUnitDTO savemappingdetails([FromBody] SchoolMasterUnitDTO data)
        {
            return _interface.savemappingdetails(data);
        }
        [Route("deactivatemapping")]
        public SchoolMasterUnitDTO deactivatemapping([FromBody] SchoolMasterUnitDTO data)
        {
            return _interface.deactivatemapping(data);
        }
        
    }
}
