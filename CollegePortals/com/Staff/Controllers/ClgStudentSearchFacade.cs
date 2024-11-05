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
    public class ClgStudentSearchFacade : Controller
    {
        public ClgStudentSearchInterface _ads;

        public ClgStudentSearchFacade(ClgStudentSearchInterface adstu)
        {
            _ads = adstu;
        }

        [HttpPost]
        [Route("getloaddata")]
        public ClgPortalAttendanceDTO getloaddata([FromBody]ClgPortalAttendanceDTO data)
        {
            return _ads.getloaddata(data);
        }
        [HttpPost]
        [Route("getcoursedata")]
        public Task<ClgPortalAttendanceDTO> getcoursedata([FromBody]ClgPortalAttendanceDTO data)
        {
            return _ads.getcoursedata(data);
        }

        [HttpPost]
        [Route("getbranchdata")]
        public Task<ClgPortalAttendanceDTO> getbranchdata([FromBody]ClgPortalAttendanceDTO data)
        {
            return _ads.getbranchdata(data);
        }
        [HttpPost]
        [Route("getsemdata")]
        public Task<ClgPortalAttendanceDTO> getsemdata([FromBody]ClgPortalAttendanceDTO data)
        {
            return _ads.getsemdata(data);
        }
        [HttpPost]
        [Route("getstudent")]
        public ClgPortalAttendanceDTO getstudent([FromBody]ClgPortalAttendanceDTO data)
        {
            return _ads.getstudent(data);
        }
        
        [HttpPost]
        [Route("getreport")]
        public Task<ClgPortalAttendanceDTO> getreport([FromBody]ClgPortalAttendanceDTO data)
        {
            return _ads.getreport(data);
        }
        
    }
}
