using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;
using HRMSServicesHub.com.vaps.Interfaces;

namespace HRMSServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class HRMasterEmpFullTimeFacadeController : Controller
    {
        public HRMasterEmpFullTimeInterface _ads;

        public HRMasterEmpFullTimeFacadeController(HRMasterEmpFullTimeInterface adstu)
        {
            _ads = adstu;
        }

        [Route("getalldetails")]
        public NAACHRMasterEmpFullTimeDTO getalldetails([FromBody]NAACHRMasterEmpFullTimeDTO dto)
        {
            return _ads.getalldetails(dto);
        }

        [Route("savedata")]
        public NAACHRMasterEmpFullTimeDTO savedata([FromBody]NAACHRMasterEmpFullTimeDTO dto)
        {
            return _ads.savedata(dto);
        }

        [Route("editRecord")]
        public NAACHRMasterEmpFullTimeDTO editRecord([FromBody]NAACHRMasterEmpFullTimeDTO dto)
        {
            return _ads.editRecord(dto);
        }

        [Route("ActiveDeactiveRecord")]
        public NAACHRMasterEmpFullTimeDTO ActiveDeactiveRecord([FromBody]NAACHRMasterEmpFullTimeDTO dto)
        {
            return _ads.ActiveDeactiveRecord(dto);
        }
    }
}
