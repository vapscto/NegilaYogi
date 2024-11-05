using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.PAOnlineExam;
using WebApplication1.PAOnlineExam.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.PAOnlineExam.Facade
{
    [Route("api/[controller]")]
    public class PAOnlineExamFacadeController : Controller
    {
        public PAOnlineExamInterface _oei;
        public PAOnlineExamFacadeController(PAOnlineExamInterface oei)
        {
            _oei = oei;
        }
        [HttpPost]
        [Route("getloaddata")]
        public PAOnlineExamDTO getloaddata([FromBody]PAOnlineExamDTO data)
        {
            return _oei.getloaddata(data);
        }

        [HttpPost]
        [Route("getSubjects")]
        public PAOnlineExamDTO getSubjects([FromBody]PAOnlineExamDTO data)
        {
            return _oei.getSubjects(data);
        }
        [HttpPost]
        [Route("getQuestion")]
        public PAOnlineExamDTO getQuestion([FromBody]PAOnlineExamDTO data)
        {
            return _oei.getQuestion(data);
        }

        [HttpPost]
        [Route("Saveanswer")]
        public PAOnlineExamDTO Saveanswer([FromBody]PAOnlineExamDTO data)
        {
            return _oei.Saveanswer(data);
        }

        [HttpPost]
        [Route("savedanswers")]
        public PAOnlineExamDTO savedanswers([FromBody]PAOnlineExamDTO data)
        {
            return _oei.savedanswers(data);
        }
        [HttpPost]
        [Route("submitexam")]
        public Task<PAOnlineExamDTO> submitexam([FromBody]PAOnlineExamDTO data)
        {
            return _oei.submitexam(data);
        }
    }
}
