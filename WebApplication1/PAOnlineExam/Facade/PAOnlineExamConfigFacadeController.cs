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
    public class PAOnlineExamConfigFacadeController : Controller
    {
        public PAOnlineExamConfigInterface _oei;
        public PAOnlineExamConfigFacadeController(PAOnlineExamConfigInterface oei)
        {
            _oei = oei;
        }

        [HttpPost]

        [Route("getloaddata")]
        public PAOnlineExamConfigDTO getloaddata([FromBody]PAOnlineExamConfigDTO data)
        {
            return _oei.getloaddata(data);
        }

        [Route("savedetails")]
        public PAOnlineExamConfigDTO savedetails([FromBody]PAOnlineExamConfigDTO data)
        {
            return _oei.savedetails(data);
        }
        [Route("editQuestion")]
        public PAOnlineExamConfigDTO editQuestion([FromBody]PAOnlineExamConfigDTO data)
        {
            return _oei.editQuestion(data);
        }

        [Route("Deletedetails")]
        public PAOnlineExamConfigDTO Deletedetails([FromBody]PAOnlineExamConfigDTO data)
        {
            return _oei.Deletedetails(data);
        }
        [Route("getreport")]
        public Task<PAOnlineExamConfigDTO> getreport([FromBody]PAOnlineExamConfigDTO data)
        {
            return _oei.getreport(data);
        }
    }
}
