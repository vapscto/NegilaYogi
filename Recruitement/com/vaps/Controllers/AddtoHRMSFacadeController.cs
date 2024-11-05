using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using Recruitment.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class AddtoHRMSFacadeController : Controller
    {
        public AddtoHRMSInterface _ads;
        public AddtoHRMSFacadeController(AddtoHRMSInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public HR_Candidate_DetailsDTO getinitialdata([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        // POST api/values
        [HttpPost]
        public HR_Candidate_DetailsDTO Post([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }

        [Route("getRecordById/{id:int}")]

        public HR_Candidate_DetailsDTO getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editData(id);
        }
        [Route("deactivateRecordById")]
        public HR_Candidate_DetailsDTO deactivateRecordById([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.deactivate(dto);
        }
        [Route("Get_Desgination")]
        public HR_Candidate_DetailsDTO Get_Desgination([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.Get_Desgination(dto);
        }
        [Route("saveAppointmentdata")]
        public HR_Candidate_DetailsDTO saveAppointmentdata([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.saveAppointmentdata(dto);
        }

        [Route("savetohrms")]
        public HR_Candidate_DetailsDTO savetohrms([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.savetohrms(dto);
        }

        [Route("getEmployeeSalaryDetailsByHead")]
        public HR_Candidate_DetailsDTO getEmployeeSalaryDetailsByHead([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.getEmployeeSalaryDetailsByHead(dto);
        }
        [Route("getcandidate")]
        public HR_Candidate_DetailsDTO getcandidate([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.getcandidate(dto);
        }  
    }
}
