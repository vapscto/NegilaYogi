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
    public class OnlineExamFacadeController : Controller
    {
        public OnlineExamInterface _oei;
        public OnlineExamFacadeController(OnlineExamInterface oei)
        {
            _oei = oei;
        }
        [HttpPost]
        [Route("getloaddata")]
        public OnlineExamDTO getloaddata([FromBody]OnlineExamDTO data)
        {
            return _oei.getloaddata(data);
        }

        [HttpPost]
        [Route("getSubjects")]
        public OnlineExamDTO getSubjects([FromBody]OnlineExamDTO data)
        {
            return _oei.getSubjects(data);
        }
        [HttpPost]
        [Route("getQuestion")]
        public OnlineExamDTO getQuestion([FromBody]OnlineExamDTO data)
        {
            return _oei.getQuestion(data);
        }

        [HttpPost]
        [Route("Saveanswer")]
        public OnlineExamDTO Saveanswer([FromBody]OnlineExamDTO data)
        {
            return _oei.Saveanswer(data);
        }
        
        [HttpPost]
        [Route("savedanswers")]
        public OnlineExamDTO savedanswers([FromBody]OnlineExamDTO data)
        {
            return _oei.savedanswers(data);
        }
        [HttpPost]
        [Route("submitexam")]
        public Task<OnlineExamDTO> submitexam([FromBody]OnlineExamDTO data)
        {
            return _oei.submitexam(data);
        }
        
    }
}
