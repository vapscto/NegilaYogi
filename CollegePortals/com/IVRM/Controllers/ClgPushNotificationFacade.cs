using CollegePortals.com.Student.Interfaces;
using DomainModel.Model;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Portals.IVRM;
using System.Threading.Tasks;

namespace CollegePortals.com.Student.Controllers
{
    [Route("api/[controller]")]
    public class ClgPushNotificationFacade : Controller
    {
        public ClgPushNotificationInterface _ads;

        public ClgPushNotificationFacade(ClgPushNotificationInterface adstu)
        {
            _ads = adstu;
        }

        [HttpPost]
        [Route("getloaddata")]      
        public Task<ClgPushNotificationDTO> getloaddata([FromBody]ClgPushNotificationDTO data)
        {
            return _ads.getloaddata(data);
        }
    
        [HttpPost]
        [Route("savedata")]
        public ClgPushNotificationDTO savedata([FromBody]ClgPushNotificationDTO data)
        {
            return _ads.savedata(data);
        }
        [HttpPost]
        [Route("getNotificationdetails")]
        public ClgPushNotificationDTO getNotificationdetails([FromBody]ClgPushNotificationDTO data)
        {
            return _ads.getNotificationdetails(data);
        }

        



    }
}
