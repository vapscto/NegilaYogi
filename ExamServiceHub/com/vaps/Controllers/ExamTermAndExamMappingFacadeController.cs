using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.Exam;
using ExamServiceHub.com.vaps.Interfaces;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ExamTermAndExamMappingFacadeController : Controller
    {
        public ExamTermAndExamMappingInterface _ttcategory;

        public ExamTermAndExamMappingFacadeController(ExamTermAndExamMappingInterface maspag)
        {
            _ttcategory = maspag;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("Getdetails")]
        public ExamTermAndExamMappingDTO Getdetails([FromBody] ExamTermAndExamMappingDTO data)//int IVRMM_Id
        {

            return _ttcategory.Getdetails(data);

        }

        [Route("editdetails/{id:int}")]
        public ExamTermAndExamMappingDTO editdetails(int ID)
        {
            return _ttcategory.editdetails(ID);
        }

        [Route("edittermmap/{id:int}")]
        public ExamTermAndExamMappingDTO edittermmap(int ID)
        {
            return _ttcategory.edittermmap(ID);
        }

        //POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [HttpPost]
        [Route("savedetail")]
        public ExamTermAndExamMappingDTO Post([FromBody] ExamTermAndExamMappingDTO org)
        {
            return _ttcategory.savedetail(org);
        }

        [HttpPost]
        [Route("savetermmap")]
        public ExamTermAndExamMappingDTO savetermmap([FromBody] ExamTermAndExamMappingDTO org)
        {
            return _ttcategory.savetermmap(org);
        }
        [HttpPost]
        [Route("ontermchange")]
        public ExamTermAndExamMappingDTO ontermchange([FromBody] ExamTermAndExamMappingDTO org)
        {
            return _ttcategory.ontermchange(org);
        }
        [HttpPost]
        [Route("get_exam")]
        public ExamTermAndExamMappingDTO get_exam([FromBody] ExamTermAndExamMappingDTO org)
        {
            return _ttcategory.get_exam(org);
        }
        [Route("getexampopup/{id:int}")]
        public ExamTermAndExamMappingDTO getexampopup(int ID)
        {
            return _ttcategory.getexampopup(ID);
        }

        [Route("deactivate")]
        public ExamTermAndExamMappingDTO deactivate([FromBody] ExamTermAndExamMappingDTO data)
        {
            return _ttcategory.deactivate(data);
        }
        [Route("deactivate1")]
        public ExamTermAndExamMappingDTO deactivate1([FromBody] ExamTermAndExamMappingDTO data)
        {
            return _ttcategory.deactivate1(data);
        }
        [Route("deactive_sub")]
        public ExamTermAndExamMappingDTO deactive_sub([FromBody] ExamTermAndExamMappingDTO data)
        {
            return _ttcategory.deactive_sub(data);
        }
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // New Coding
        [Route("onchangeyear")]
        public ExamTermAndExamMappingDTO onchangeyear([FromBody] ExamTermAndExamMappingDTO data)
        {
            return _ttcategory.onchangeyear(data);
        }
        [Route("onchangecategory")]
        public ExamTermAndExamMappingDTO onchangecategory([FromBody] ExamTermAndExamMappingDTO data)
        {
            return _ttcategory.onchangecategory(data);
        }
        [Route("checktermname")]
        public ExamTermAndExamMappingDTO checktermname([FromBody] ExamTermAndExamMappingDTO data)
        {
            return _ttcategory.checktermname(data);
        }
        [Route("saveddata")]
        public ExamTermAndExamMappingDTO saveddata([FromBody] ExamTermAndExamMappingDTO data)
        {
            return _ttcategory.saveddata(data);
        }
        [Route("editdetailsnew")]
        public ExamTermAndExamMappingDTO editdetailsnew([FromBody] ExamTermAndExamMappingDTO data)
        {
            return _ttcategory.editdetailsnew(data);
        }
        [Route("viewrecordspopup")]
        public ExamTermAndExamMappingDTO viewrecordspopup([FromBody] ExamTermAndExamMappingDTO data)
        {
            return _ttcategory.viewrecordspopup(data);
        }
        [Route("deactivatenew")]
        public ExamTermAndExamMappingDTO deactivatenew([FromBody] ExamTermAndExamMappingDTO data)
        {
            return _ttcategory.deactivatenew(data);
        }
        [Route("deactivesub")]
        public ExamTermAndExamMappingDTO deactivesub([FromBody] ExamTermAndExamMappingDTO data)
        {
            return _ttcategory.deactivesub(data);
        }

        
    }
}
