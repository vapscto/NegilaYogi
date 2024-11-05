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
    public class  ClgStaffDashboardFacade : Controller
    {
        public  ClgStaffDashboardInterface _ads;

        public  ClgStaffDashboardFacade( ClgStaffDashboardInterface adstu)
        {
            _ads = adstu;
        }

        [HttpPost]
        [Route("getloaddata")]
        public Task<ClgStaffDashboardDTO> getloaddata([FromBody]ClgStaffDashboardDTO data)
        {
            return _ads.getloaddata(data);
        }
       
    }
}
