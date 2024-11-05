using CollegePortals.com.Staff.Interfaces;
using CollegePortals.com.Student.Interfaces;
using DomainModel.Model;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Portals.Staff;
using System.Threading.Tasks;

namespace CollegePortals.com.Staff.Controllers
{
    [Route("api/[controller]")]
    public class ClgSalaryDetailsFacade : Controller
    {
        public ClgSalaryDetailsInterface _ads;

        public ClgSalaryDetailsFacade(ClgSalaryDetailsInterface adstu)
        {
            _ads = adstu;
        }

        [HttpPost]
        [Route("getloaddata")]      
        public ClgPortalHRMSDTO getloaddata([FromBody]ClgPortalHRMSDTO data)
        {
            return _ads.getloaddata(data);
        }
        [HttpPost]
        [Route("getSalary")]
        public ClgPortalHRMSDTO getSalary([FromBody]ClgPortalHRMSDTO data)
        {
            return _ads.getSalary(data);
        }
        [HttpPost]
        [Route("getsalaryalldetails")]
        public ClgPortalHRMSDTO getsalaryalldetails([FromBody]ClgPortalHRMSDTO data)
        {
            return _ads.getsalaryalldetails(data);
        }
        

    }
}
