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
    public class PAMasterQuestionFacadeController : Controller
    {
        public PAMasterQuestionInterface _oei;
        public PAMasterQuestionFacadeController(PAMasterQuestionInterface oei)
        {
            _oei = oei;
        }
        [HttpPost]
        [Route("getloaddata")]
        public PAMasterQuestionDTO getloaddata([FromBody]PAMasterQuestionDTO data)
        {
            return _oei.getloaddata(data);
        }

        //--------------------------1st Tab
        [Route("savedetails")]
        public PAMasterQuestionDTO savedetails([FromBody]PAMasterQuestionDTO data)
        {
            return _oei.savedetails(data);
        }
        [Route("viewdocumetns")]
        public PAMasterQuestionDTO viewdocumetns([FromBody]PAMasterQuestionDTO data)
        {
            return _oei.viewdocumetns(data);
        }
        [Route("deactiveparticulars")]
        public PAMasterQuestionDTO deactiveparticulars([FromBody]PAMasterQuestionDTO data)
        {
            return _oei.deactiveparticulars(data);
        }



        //--------------------------2st Tab
        [Route("savedataclass")]
        public PAMasterQuestionDTO savedataclass([FromBody]PAMasterQuestionDTO data)
        {
            return _oei.savedataclass(data);
        }
        [Route("editQuestion")]
        public PAMasterQuestionDTO editQuestion([FromBody]PAMasterQuestionDTO data)
        {
            return _oei.editQuestion(data);
        }
        //--------------------------2nd Tab
        [Route("savedetails1")]
        public PAMasterQuestionDTO savedetails1([FromBody]PAMasterQuestionDTO data)
        {
            return _oei.savedetails1(data);
        }
        [Route("optionChange")]
        public PAMasterQuestionDTO optionChange([FromBody]PAMasterQuestionDTO data)
        {
            return _oei.optionChange(data);
        }

        [Route("optiondetails")]
        public PAMasterQuestionDTO optiondetails([FromBody]PAMasterQuestionDTO data)
        {
            return _oei.optiondetails(data);
        }

        [Route("Deletedetails")]
        public PAMasterQuestionDTO Deletedetails([FromBody]PAMasterQuestionDTO data)
        {
            return _oei.Deletedetails(data);
        }
    }
}
