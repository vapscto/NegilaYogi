using CollegePortals.com.Student.Interfaces;
using DomainModel.Model;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Portals.Student;
using System.Threading.Tasks;

namespace CollegePortals.com.Student.Controllers
{
    [Route("api/[controller]")]
    public class ClgAttendanceDetailsFacade : Controller
    {
        public ClgAttendanceDetailsInterface _ads;

        public ClgAttendanceDetailsFacade(ClgAttendanceDetailsInterface adstu)
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
        [Route("getAttdata")]
        public Task<ClgPortalAttendanceDTO> getAttdata([FromBody]ClgPortalAttendanceDTO sddto)
        {
            return _ads.getAttdata(sddto);
        }
        [HttpPost]
        [Route("MblgetAttdata")]
        public Task<ClgPortalAttendanceDTO> MblgetAttdata([FromBody]ClgPortalAttendanceDTO sddto)
        {
            return _ads.MblgetAttdata(sddto);
        }

    }
}
