using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using Recruitment.com.vaps.Interfaces;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class CandidateInterviewListVMSFacadeController : Controller
    {

        public CandidateInterviewListVMSInterface _ads;

        public CandidateInterviewListVMSFacadeController(CandidateInterviewListVMSInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public HR_CandidateInterviewScheduleDTO getinitialdata([FromBody]HR_CandidateInterviewScheduleDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        // POST api/values
        [HttpPost]
        public HR_CandidateInterviewScheduleDTO Post([FromBody]HR_CandidateInterviewScheduleDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }

        [Route("getRecordById/{id:int}")]

        public Task<HR_CandidateInterviewScheduleDTO> getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editDataAsync(id);
        }

        [Route("getallwithoutcondtn")]
        public HR_CandidateInterviewScheduleDTO getallwithoutcondtn([FromBody]HR_CandidateInterviewScheduleDTO dto)
        {
            return _ads.getallwithoutcondtn(dto);
        }

        [Route("deactivateRecordById")]
        public HR_CandidateInterviewScheduleDTO deactivateRecordById([FromBody]HR_CandidateInterviewScheduleDTO dto)
        {
            return _ads.deactivateRecordById(dto);
        }
    }
}
