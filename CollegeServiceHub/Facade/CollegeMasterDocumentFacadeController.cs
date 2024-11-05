using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeServiceHub.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class CollegeMasterDocumentFacadeController : Controller
    {
        public CollegeMasterDocumentInterface _interface;
        public CollegeMasterDocumentFacadeController(CollegeMasterDocumentInterface dinterface)
        {
            _interface = dinterface;
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
        // First Tab

        [Route("Getdetails")]
        public CollegeDocumentMasterDTO Getdetails([FromBody] CollegeDocumentMasterDTO data)
        {
            return _interface.Getdetails(data);
        }
        [Route("savedata")]
        public CollegeDocumentMasterDTO savedata([FromBody]CollegeDocumentMasterDTO data)
        {
            return _interface.savedata(data);
        }
        [Route("Editdata")]
        public CollegeDocumentMasterDTO Editdata([FromBody]CollegeDocumentMasterDTO data)
        {
            return _interface.Editdata(data);
        }
        [Route("DeleteData")]
        public CollegeDocumentMasterDTO DeleteData([FromBody]CollegeDocumentMasterDTO data)
        {
            return _interface.DeleteData(data);
        }
        // End Of First Tab
        [Route("onchangecourse")]
        public CollegeDocumentMasterDTO onchangecourse([FromBody]CollegeDocumentMasterDTO data)
        {
            return _interface.onchangecourse(data);
        }
        [Route("savedata1")]
        public CollegeDocumentMasterDTO savedata1([FromBody]CollegeDocumentMasterDTO data)
        {
            return _interface.savedata1(data);
        }
        [Route("getdoc")]
        public CollegeDocumentMasterDTO getdoc([FromBody]CollegeDocumentMasterDTO data)
        {
            return _interface.getdoc(data);
        }
        [Route("deactive_sub")]
        public CollegeDocumentMasterDTO deactive_sub([FromBody]CollegeDocumentMasterDTO data)
        {
            return _interface.deactive_sub(data);
        }        
    }
}
