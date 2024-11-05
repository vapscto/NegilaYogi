using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using WebApplication1.Interfaces;
using PreadmissionDTOs.com.vaps.OnlineExam;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class MasterQuestionCollegeFacadeController : Controller
    {
        public MasterQuestionCollegeInterface _oei;
        public MasterQuestionCollegeFacadeController(MasterQuestionCollegeInterface oei)
        {
            _oei = oei;
        }
        [HttpPost]
        [Route("getloaddata")]
        public MasterQuestionDTO getloaddata([FromBody]MasterQuestionDTO data)
        {
            return _oei.getloaddata(data);
        }

        //--------------------------1st Tab
        [Route("savedetails")]
        public MasterQuestionDTO savedetails([FromBody]MasterQuestionDTO data)
        {
            return _oei.savedetails(data);
        }

        

        //--------------------------2st Tab
        [Route("savedataclass")]
        public MasterQuestionDTO savedataclass([FromBody]MasterQuestionDTO data)
        {
            return _oei.savedataclass(data);
        }
        [Route("editQuestion")]
        public MasterQuestionDTO editQuestion([FromBody]MasterQuestionDTO data)
        {
            return _oei.editQuestion(data);
        }
        //--------------------------2nd Tab
        [Route("savedetails1")]
        public MasterQuestionDTO savedetails1([FromBody]MasterQuestionDTO data)
        {
            return _oei.savedetails1(data);
        }
        [Route("optionChange")]
        public MasterQuestionDTO optionChange([FromBody]MasterQuestionDTO data)
        {
            return _oei.optionChange(data);
        }

        [Route("optiondetails")]
        public MasterQuestionDTO optiondetails([FromBody]MasterQuestionDTO data)
        {
            return _oei.optiondetails(data);
        }

        [Route("Deletedetails")]
        public MasterQuestionDTO Deletedetails([FromBody]MasterQuestionDTO data)
        {
            return _oei.Deletedetails(data);
        }
        [Route("selectcourse")]
        public MasterQuestionDTO selectcourse([FromBody]MasterQuestionDTO data)
        {
            return _oei.selectcourse(data);
        }
        [Route("selectbran")]
        public MasterQuestionDTO selectbran([FromBody]MasterQuestionDTO data)
        {
            return _oei.selectbran(data);
        }
        [Route("editbranchquestion")]
        public MasterQuestionDTO editbranchquestion([FromBody]MasterQuestionDTO data)
        {
            return _oei.editbranchquestion(data);
        }
        

    }
}
