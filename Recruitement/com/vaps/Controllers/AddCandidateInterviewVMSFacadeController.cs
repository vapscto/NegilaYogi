using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using Recruitment.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class AddCandidateInterviewVMSFacadeController : Controller
    {

        public AddCandidateInterviewVMSInterface _ads;

        public AddCandidateInterviewVMSFacadeController(AddCandidateInterviewVMSInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public HR_CandidateInterviewScheduleDTO getinitialdata([FromBody]HR_CandidateInterviewScheduleDTO dto)
        {
            return _ads.getBasicData(dto);
        }
        [Route("getallgrade")]
        public HR_CandidateInterviewScheduleDTO getallgrade([FromBody]HR_CandidateInterviewScheduleDTO dto)
        {
            return _ads.getallgrade(dto);
        }

        // POST api/values
        [HttpPost]
        public HR_CandidateInterviewScheduleDTO Post([FromBody]HR_CandidateInterviewScheduleDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }

        [Route("getRecordById/{id:int}")]

        public HR_CandidateInterviewScheduleDTO getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editData(id);
        }
        [Route("deactivateRecordById")]
        public HR_CandidateInterviewScheduleDTO deactivateRecordById([FromBody]HR_CandidateInterviewScheduleDTO dto)
        {
            return _ads.deactivate(dto);
        }

        [Route("savefeedback")]
        public HR_CandidateInterviewScheduleDTO savefeedback([FromBody]HR_CandidateInterviewScheduleDTO dto)
        {
            return _ads.savefeedback(dto);
        }

        [Route("getrpt")]
        public HR_CandidateInterviewScheduleDTO getrpt([FromBody]HR_CandidateInterviewScheduleDTO dto)
        {
            return _ads.getrpt(dto);
        }
    }
}
