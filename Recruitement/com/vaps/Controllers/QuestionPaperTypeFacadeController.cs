using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using Recruitment.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class QuestionPaperTypeFacadeController : Controller
    {

        public QuestionPaperTypeInterface _ads;

        public QuestionPaperTypeFacadeController(QuestionPaperTypeInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("getalldetails")]
        public QuestionPaperTypeDTO getalldetails([FromBody]QuestionPaperTypeDTO dto)
        {
            return _ads.getalldetails(dto);
        }

        // POST api/values
        [HttpPost]
        [Route("savedetails")]
        public QuestionPaperTypeDTO savedetails([FromBody]QuestionPaperTypeDTO dto)
        {
            return _ads.savedetails(dto);
        }

        [Route("editData/{id:int}")]

        public QuestionPaperTypeDTO editData(int id)
        {
            return _ads.editData(id);
        }
        [Route("deactivate")]
        public QuestionPaperTypeDTO deactivate([FromBody]QuestionPaperTypeDTO dto)
        {
            return _ads.deactivate(dto);
        }
    }
}
