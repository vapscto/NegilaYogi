using CollegePortals.com.Staff.Interfaces;
using CollegePortals.com.Student.Interfaces;
using DomainModel.Model;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Portals.Staff;
using PreadmissionDTOs.com.vaps.College.Portals.Student;
using System.Threading.Tasks;

namespace CollegePortals.com.Staff.Controllers
{
    [Route("api/[controller]")]
    public class ClgLiveMeetingScheduleFacade : Controller
    {
        public ClgLiveMeetingScheduleInterface _ads;

        public ClgLiveMeetingScheduleFacade(ClgLiveMeetingScheduleInterface adstu)
        {
            _ads = adstu;
        }

        [HttpPost]
        [Route("getloaddata")]
        public ClgLiveMeetingScheduleDTO getloaddata([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            return _ads.getloaddata(data);
        }
        [HttpPost]
        [Route("getcoursedata")]
        public Task<ClgLiveMeetingScheduleDTO> getcoursedata([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            return _ads.getcoursedata(data);
        }

        [HttpPost]
        [Route("getbranchdata")]
        public Task<ClgLiveMeetingScheduleDTO> getbranchdata([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            return _ads.getbranchdata(data);
        }
        [HttpPost]
        [Route("getsection")]
        public Task<ClgLiveMeetingScheduleDTO> getsection([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            return _ads.getsection(data);
        }
        [HttpPost]
        [Route("getsemdata")]
        public Task<ClgLiveMeetingScheduleDTO> getsemdata([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            return _ads.getsemdata(data);
        }
        [HttpPost]
        [Route("editdata")]
        public ClgLiveMeetingScheduleDTO editdata([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            return _ads.editdata(data);
        }
        
        [HttpPost]
        [Route("savedata")]
        public Task<ClgLiveMeetingScheduleDTO> savedata([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            return _ads.savedata(data);
        }
        [HttpPost]
        [Route("deactive")]
        public Task<ClgLiveMeetingScheduleDTO> deactive([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            return _ads.deactive(data);
        }
        //STAFF PROFILE
        [HttpPost]
        [Route("getempdetails")]
        public Task<ClgLiveMeetingScheduleDTO> getempdetails([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            return _ads.getempdetails(data);
        }
        [HttpPost]
        [Route("ondatechange")]
        public Task<ClgLiveMeetingScheduleDTO> ondatechange([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            return _ads.ondatechange(data);
        }
        [HttpPost]
        [Route("onstartmeeting")]
        public Task<ClgLiveMeetingScheduleDTO> onstartmeeting([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            return _ads.onstartmeeting(data);
        }
        [HttpPost]
        [Route("endmainmeeting")]
        public Task<ClgLiveMeetingScheduleDTO> endmainmeeting([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            return _ads.endmainmeeting(data);
        }
         [HttpPost]
        [Route("joinmeeting")]
        public Task<ClgLiveMeetingScheduleDTO> joinmeeting([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            return _ads.joinmeeting(data);
        }
        [HttpPost]

        [Route("getstudentdetails")]
        public Task<ClgLiveMeetingScheduleDTO> getstudentdetails([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            return _ads.getstudentdetails(data);
        }

        [HttpPost]

        [Route("endmainmeetingstudent")]
        public Task<ClgLiveMeetingScheduleDTO> endmainmeetingstudent([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            return _ads.endmainmeetingstudent(data);
        }
        [HttpPost]

        [Route("joinmeetingStudent")]
        public Task<ClgLiveMeetingScheduleDTO> joinmeetingStudent([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            return _ads.joinmeetingStudent(data);
        }
        [HttpPost]

        [Route("ondatechangestudent")]
        public Task<ClgLiveMeetingScheduleDTO> ondatechangestudent([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            return _ads.ondatechangestudent(data);
        }

        [Route("getschrptdetails")]
        public Task<ClgLiveMeetingScheduleDTO> getschrptdetails([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            return _ads.getschrptdetails(data);
        }
        [Route("getschrptdetailsprofile")]
        public Task<ClgLiveMeetingScheduleDTO> getschrptdetailsprofile([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            return _ads.getschrptdetailsprofile(data);
        }
        [Route("getschedulereport")]
        public Task<ClgLiveMeetingScheduleDTO> getschedulereport([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            return _ads.getschedulereport(data);
        }
        [Route("getstaffprofilereport")]
        public Task<ClgLiveMeetingScheduleDTO> getstaffprofilereport([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            return _ads.getstaffprofilereport(data);
        }
        [Route("getstudentprofiledata")]
        public Task<ClgLiveMeetingScheduleDTO> getstudentprofiledata([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            return _ads.getstudentprofiledata(data);
        }
        [Route("getstudentprofilereport")]
        public Task<ClgLiveMeetingScheduleDTO> getstudentprofilereport([FromBody]ClgLiveMeetingScheduleDTO data)
        {
            return _ads.getstudentprofilereport(data);
        }
        
    }
}
