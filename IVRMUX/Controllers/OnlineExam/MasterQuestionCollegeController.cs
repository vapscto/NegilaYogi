using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using corewebapi18072016.Delegates.com.vapstech.OnlineExam;
using PreadmissionDTOs.com.vaps.OnlineExam;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterQuestionCollege : Controller
    {
        MasterQuestionCollegeDelegate oed = new MasterQuestionCollegeDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
       
        [Route("getloaddata/{id:int}")]
        public MasterQuestionDTO getloaddata(int id)
        {
            MasterQuestionDTO data = new MasterQuestionDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return oed.getloaddata(data);
        }
        //----------------------------1stTab
        [Route("savedetails")]
        public MasterQuestionDTO savedetails([FromBody] MasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return oed.savedetails(data);
        }


        //----2nd tab

        [Route("savedataclass")]
        public MasterQuestionDTO savedataclass([FromBody] MasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return oed.savedataclass(data);
        }



        [Route("editQuestion")]
        public MasterQuestionDTO editQuestion([FromBody] MasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return oed.editQuestion(data);
        }
        //-----------------------------3nd Tab
        [Route("savedetails1")]
        public MasterQuestionDTO savedetails1([FromBody] MasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return oed.savedetails1(data);
        }
        [Route("optionChange")]
        public MasterQuestionDTO optionChange([FromBody] MasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return oed.optionChange(data);
        }
        [Route("optiondetails")]
        public MasterQuestionDTO optiondetails([FromBody] MasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return oed.optiondetails(data);
        }

        [Route("Deletedetails")]
        public MasterQuestionDTO Deletedetails([FromBody] MasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return oed.Deletedetails(data);
        }
        [Route("selectcourse")]
        public MasterQuestionDTO selectcourse([FromBody] MasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return oed.selectcourse(data);
        }
        [Route("selectbran")]
        public MasterQuestionDTO selectbran([FromBody] MasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return oed.selectbran(data);
        }
        [Route("editbranchquestion")]
        public MasterQuestionDTO editbranchquestion([FromBody] MasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return oed.editbranchquestion(data);
        }




    }
}
