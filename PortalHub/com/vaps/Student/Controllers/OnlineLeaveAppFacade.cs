using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Student.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Student;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Student.Controllers
{
    [Route("api/[controller]")]
    public class OnlineLeaveAppFacade : Controller
    {
        // GET: api/<controller>

        public OnlineLeaveAppInterface _ads;

        public OnlineLeaveAppFacade(OnlineLeaveAppInterface adstu)
        {
            _ads = adstu;
        }



        [Route("getdetails")]
        public OnlineLeaveApp_DTO getdetails([FromBody]OnlineLeaveApp_DTO sddto)
        {
            return _ads.getdetails(sddto);
        }

        [Route("leaveapply")]
        public Task<OnlineLeaveApp_DTO> leaveapply([FromBody]OnlineLeaveApp_DTO sddto)
        {
            return _ads.leaveapply(sddto);
        }

        [Route("editdata")]
        public OnlineLeaveApp_DTO editdata([FromBody]OnlineLeaveApp_DTO sddto)
        {
            return _ads.editdata(sddto);
        }

        [Route("leaveApproved")]
        public Task<OnlineLeaveApp_DTO> leaveApproved([FromBody]OnlineLeaveApp_DTO sddto)
        {
            return _ads.leaveApproved(sddto);
        }

        [Route("leaveRejected")]
        public Task<OnlineLeaveApp_DTO> leaveRejected([FromBody]OnlineLeaveApp_DTO sddto)
        {
            return _ads.leaveRejected(sddto);
        }

        [Route("deactiveY")]
        public OnlineLeaveApp_DTO deactiveY([FromBody]OnlineLeaveApp_DTO sddto)
        {
            return _ads.deactiveY(sddto);
        }

        [Route("cancellationRecord")]
        public OnlineLeaveApp_DTO cancellationRecord([FromBody]OnlineLeaveApp_DTO sddto)
        {
            return _ads.cancellationRecord(sddto);
        }
        [Route("getdate_sla")]
        public OnlineLeaveApp_DTO getdate_sla([FromBody]OnlineLeaveApp_DTO sddto)
        {
            return _ads.getdate_sla(sddto);
        }
        [Route("getsection")]
        public OnlineLeaveApp_DTO getsection([FromBody]OnlineLeaveApp_DTO sddto)
        {
            return _ads.getsection(sddto);
        }
        [Route("getstudent")]
        public OnlineLeaveApp_DTO getstudent([FromBody]OnlineLeaveApp_DTO sddto)
        {
            return _ads.getstudent(sddto);
        }
        [Route("get_leave_Report")]
        public OnlineLeaveApp_DTO get_leave_Report([FromBody]OnlineLeaveApp_DTO sddto)
        {
            return _ads.get_leave_Report(sddto);
        }
        [Route("get_TC_Report")]
        public TransferCertificate_DTO get_TC_Report([FromBody]TransferCertificate_DTO sddto)
        {
            return _ads.get_TC_Report(sddto);
        }
        
    }
}
