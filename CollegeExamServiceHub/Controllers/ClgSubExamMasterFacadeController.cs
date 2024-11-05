using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeExamServiceHub.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeExamServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class ClgSubExamMasterFacadeController : Controller
    {
        ClgSubExamMasterInterface _mastersubexam;
        public ClgSubExamMasterFacadeController(ClgSubExamMasterInterface mastersubexam)
        {
            _mastersubexam = mastersubexam;
        }
        [Route("Getdetails")]
        public mastersubexamDTO Getdetails([FromBody]mastersubexamDTO data)
        {
            return _mastersubexam.Getdetails(data);
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }      
        [Route("savedetails")]
        public mastersubexamDTO savedetails([FromBody] mastersubexamDTO data)
        {
            return _mastersubexam.savedetails(data);
        }
        [Route("editdetails/{id:int}")]
        public mastersubexamDTO editdetails(int ID)
        {
            return _mastersubexam.editdetails(ID);
        }
        [Route("validateordernumber")]
        public mastersubexamDTO validateordernumber([FromBody] mastersubexamDTO data)
        {
            return _mastersubexam.validateordernumber(data);
        }


        [Route("deactivate")]
        public mastersubexamDTO deactivate([FromBody] mastersubexamDTO data)
        {
            return _mastersubexam.deactivate(data);
        }
    }
}
