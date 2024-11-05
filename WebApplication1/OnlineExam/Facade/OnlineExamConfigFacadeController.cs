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
    public class OnlineExamConfigFacadeController : Controller
    {
        public OnlineExamConfigInterface _oei;
        public OnlineExamConfigFacadeController(OnlineExamConfigInterface oei)
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
        [Route("editQuestion")]
        public MasterQuestionDTO editQuestion([FromBody]MasterQuestionDTO data)
        {
            return _oei.editQuestion(data);
        }
        
        [Route("Deletedetails")]
        public MasterQuestionDTO Deletedetails([FromBody]MasterQuestionDTO data)
        {
            return _oei.Deletedetails(data);
        }
        [Route("getreport")]
        public Task<MasterQuestionDTO> getreport([FromBody]MasterQuestionDTO data)
        {
            return _oei.getreport(data);
        }
        //=====================online exam report new

       
        [Route("getsection")]
        public MasterQuestionDTO getsection([FromBody]MasterQuestionDTO data)
        {
            return _oei.getsection(data);
        }

         [Route("getonlinereport")]
        public MasterQuestionDTO getonlinereport([FromBody]MasterQuestionDTO data)
        {
            return _oei.getonlinereport(data);
        }


       

    }
}
