using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.PAOnlineExam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.PAOnlineExam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.PAOnlineExam
{
    [Route("api/[controller]")]
    public class PAMasterQuestionController : Controller
    {
        PAMasterQuestionDelegate oed = new PAMasterQuestionDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getloaddata/{id:int}")]
        public PAMasterQuestionDTO getloaddata(int id)
        {
            PAMasterQuestionDTO data = new PAMasterQuestionDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return oed.getloaddata(data);
        }
        //----------------------------1stTab
        [Route("savedetails")]
        public PAMasterQuestionDTO savedetails([FromBody] PAMasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return oed.savedetails(data);
        }

        [Route("viewdocumetns")]
        public PAMasterQuestionDTO viewdocumetns([FromBody] PAMasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return oed.viewdocumetns(data);
        }

        [Route("deactiveparticulars")]
        public PAMasterQuestionDTO deactiveparticulars([FromBody] PAMasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return oed.deactiveparticulars(data);
        }


        //----2nd tab

        [Route("savedataclass")]
        public PAMasterQuestionDTO savedataclass([FromBody] PAMasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return oed.savedataclass(data);
        }

        [Route("editQuestion")]
        public PAMasterQuestionDTO editQuestion([FromBody] PAMasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return oed.editQuestion(data);
        }

        //-----------------------------3nd Tab
        [Route("savedetails1")]
        public PAMasterQuestionDTO savedetails1([FromBody] PAMasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return oed.savedetails1(data);
        }
        [Route("optionChange")]
        public PAMasterQuestionDTO optionChange([FromBody] PAMasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return oed.optionChange(data);
        }
        [Route("optiondetails")]
        public PAMasterQuestionDTO optiondetails([FromBody] PAMasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return oed.optiondetails(data);
        }

        [Route("Deletedetails")]
        public PAMasterQuestionDTO Deletedetails([FromBody] PAMasterQuestionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return oed.Deletedetails(data);
        }
    }
}