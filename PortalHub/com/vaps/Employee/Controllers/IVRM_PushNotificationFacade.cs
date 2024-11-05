using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Employee.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Employee;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Employee.Controllers
{
    [Route("api/[controller]")]
    public class IVRM_PushNotificationFacade : Controller
    {

        // GET: api/values
        public IVRM_PushNotificationInterface _msg;
        public IVRM_PushNotificationFacade(IVRM_PushNotificationInterface msg)
        {
            _msg = msg;
        }
        // GET: api/values
        [Route("savedetail")]
        public IVRM_PushNotificationDTO savedetail([FromBody]IVRM_PushNotificationDTO data)
        {
            return _msg.savedetail(data);
        }
        [Route("Getdetails")]
        public Task<IVRM_PushNotificationDTO> Getdetails([FromBody]IVRM_PushNotificationDTO data)
        {
            return _msg.Getdetails(data);
        }
        [Route("deactivate")]
        public IVRM_PushNotificationDTO deactivate([FromBody]IVRM_PushNotificationDTO data)
        {
            return _msg.deactivate(data);
        }
       
        [Route("editnoticestf")]
        public IVRM_PushNotificationDTO editnoticestf([FromBody] IVRM_PushNotificationDTO data)
        {
            return _msg.editnoticestf(data);
        }

        [Route("editnoticestud")]
        public IVRM_PushNotificationDTO editnoticestud([FromBody] IVRM_PushNotificationDTO data)
        {
            return _msg.editnoticestud(data);
        }

        [Route("Deactivatemain")]
        public IVRM_PushNotificationDTO Deactivatemain([FromBody]IVRM_PushNotificationDTO data)
        {
            return _msg.Deactivatemain(data);
        }

        [Route("Deactivatestud")]
        public IVRM_PushNotificationDTO Deactivatestud([FromBody]IVRM_PushNotificationDTO data)
        {
            return _msg.Deactivatestud(data);
        }

        [Route("get_modeldata")]
        public IVRM_PushNotificationDTO get_modeldata([FromBody]IVRM_PushNotificationDTO data)
        {
            return _msg.get_modeldata(data);
        }


        
    }
}
