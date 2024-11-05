using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;
using HRMSServicesHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ESIReportFacadeController : Controller
    {
        // GET: api/values
        public ESIReportInterface _ads;

        public ESIReportFacadeController(ESIReportInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public ESIReportDTO getinitialdata([FromBody]ESIReportDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        [Route("getEmployeedetailsBySelection")]
        public Task<ESIReportDTO> getEmployeedetailsBySelection([FromBody]ESIReportDTO dto)
        {
            return _ads.getEmployeedetailsBySelection(dto);
        }

        [Route("get_depts")]
        public ESIReportDTO get_depts([FromBody]ESIReportDTO dto)
        {
            return _ads.get_depts(dto);
        }
        [Route("get_desig")]
        public ESIReportDTO get_desig([FromBody]ESIReportDTO dto)
        {
            return _ads.get_desig(dto);
        }
    }
}
