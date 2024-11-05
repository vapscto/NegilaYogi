using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.COE;
using PreadmissionDTOs.com.vaps.COE;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Exam;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Exam
{
    [Route("api/[controller]")]
    public class ExamTermAndExamMapping : Controller
    {
        ExamTermAndExamMappingDelegates objdelegate = new ExamTermAndExamMappingDelegates();


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5

        [Route("Getdetails")]
        public ExamTermAndExamMappingDTO Getdetails(ExamTermAndExamMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return objdelegate.Getdetails(data);
        }

        [Route("editdetails/{id:int}")]
        public ExamTermAndExamMappingDTO editdetails(int ID)
        {
            return objdelegate.editdetails(ID);
        }

        [Route("edittermmap/{id:int}")]
        public ExamTermAndExamMappingDTO edittermmap(int ID)
        {
            return objdelegate.edittermmap(ID);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [HttpPost]
        [Route("savedetails")]
        public ExamTermAndExamMappingDTO savedetail([FromBody] ExamTermAndExamMappingDTO TermAndMap)
        {
            //  categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            //categorypage.EMGR_MarksPerFlag = Convert.ToChar(categorypage.EMGR_MarksPerFlag);
            TermAndMap.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail(TermAndMap);
        }

        [HttpPost]
        [Route("savetermmap")]
        public ExamTermAndExamMappingDTO savetermmap([FromBody] ExamTermAndExamMappingDTO TermAndMap)
        {
            TermAndMap.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savetermmap(TermAndMap);
        }

        [HttpPost]
        [Route("ontermchange")]
        public ExamTermAndExamMappingDTO ontermchange([FromBody] ExamTermAndExamMappingDTO TermAndMap)
        {

            TermAndMap.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.ontermchange(TermAndMap);
        }
        [HttpPost]
        [Route("get_exam")]
        public ExamTermAndExamMappingDTO get_exam([FromBody] ExamTermAndExamMappingDTO TermAndMap)
        {
            TermAndMap.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.get_exam(TermAndMap);
        }

        [Route("getexampopup/{id:int}")]
        public ExamTermAndExamMappingDTO getexampopup(int ID)
        {
            return objdelegate.getexampopup(ID);
        }

        [Route("deactivate")]
        public ExamTermAndExamMappingDTO deactivate([FromBody] ExamTermAndExamMappingDTO data)
        {
            return objdelegate.deactivate(data);
        }
        [Route("deactivate1/{id:int}")]
        public ExamTermAndExamMappingDTO deactivate1(int id)
        {
            ExamTermAndExamMappingDTO data = new ExamTermAndExamMappingDTO();
            data.ECTMP_Id = id;
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.deactivate1(data);
        }

        [Route("deactive_sub")]
        public ExamTermAndExamMappingDTO deactive_sub([FromBody] ExamTermAndExamMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.deactive_sub(data);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // New coding 
        [Route("onchangeyear")]
        public ExamTermAndExamMappingDTO onchangeyear([FromBody] ExamTermAndExamMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.onchangeyear(data);
        }
        [Route("onchangecategory")]
        public ExamTermAndExamMappingDTO onchangecategory([FromBody] ExamTermAndExamMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.onchangecategory(data);
        }
        [Route("checktermname")]
        public ExamTermAndExamMappingDTO checktermname([FromBody] ExamTermAndExamMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.checktermname(data);
        }
        [Route("saveddata")]
        public ExamTermAndExamMappingDTO saveddata([FromBody] ExamTermAndExamMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.saveddata(data);
        }
        [Route("editdetailsnew")]
        public ExamTermAndExamMappingDTO editdetailsnew([FromBody] ExamTermAndExamMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.editdetailsnew(data);
        }
        [Route("viewrecordspopup")]
        public ExamTermAndExamMappingDTO viewrecordspopup([FromBody] ExamTermAndExamMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.viewrecordspopup(data);
        }

        [Route("deactivatenew")]
        public ExamTermAndExamMappingDTO deactivatenew([FromBody] ExamTermAndExamMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.deactivatenew(data);
        }
        [Route("deactivesub")]
        public ExamTermAndExamMappingDTO deactivesub([FromBody] ExamTermAndExamMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.deactivesub(data);
        }
        

    }
}
