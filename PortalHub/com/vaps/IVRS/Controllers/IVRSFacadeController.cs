using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.IVRS;
using PortalHub.com.vaps.IVRS.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.IVRS.Controllers
{
    [Route("api/[controller]")]
    public class IVRSFacadeController : Controller
    {
      
        IVRSInterface _intf;
        public IVRSFacadeController(IVRSInterface intf)
        {
            _intf = intf;
        }
        [Route("getdata")]
        public JsonResult getdata([FromBody] IVRSDTO data)
        {
            return _intf.getdata(data);
        }
        [Route("getbranch")]
        public JsonResult getbranch([FromBody] IVRSDTO data)
        {
            return _intf.getbranch(data);
        }
        [Route("updatecredits")]
        public JsonResult updatecredits([FromBody] IVRSDTO data)
        {
            return _intf.updatecredits(data);
        }
        [Route("UpdateMobile")]
        public JsonResult UpdateMobile([FromBody] IVRSDTO data)
        {
            return _intf.UpdateMobile(data);
        }

        [Route("savedetail")]
        public IVRM_IVRS_ConfigurationDTO savedetail([FromBody] IVRM_IVRS_ConfigurationDTO data)
        {
            return _intf.savedetail(data);
        }
        [Route("getdetails")]
        public IVRSDTO getdetails([FromBody] IVRSDTO data)
        {
            return _intf.getdetails(data);
        }
        [Route("getpagedetails/{id:int}")]
        public IVRSDTO getpagedetails(int id)
        {
            return _intf.getpageedit(id);
        }
        [Route("getdetails_page/{id:int}")]
        public IVRM_IVRS_ConfigurationDTO getdetails_page(int id)
        {
            return _intf.getdetails_page(id);
        }
        [HttpPost]
        [Route("deactivate")]
        public IVRM_IVRS_ConfigurationDTO deactivateAcdmYear([FromBody] IVRM_IVRS_ConfigurationDTO id)
        {
            return _intf.deactivate(id);
        }

        [HttpPost]
        [Route("student_staff_notification")]

        public IVRM_IVRS_ConfigurationDTO student_staff_notification([FromBody] IVRM_IVRS_ConfigurationDTO id)
        {
            return _intf.student_staff_notification(id);
        }
    }
}
