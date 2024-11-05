using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRMSServicesHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class PS7andPS8FormReportFacadeController : Controller
    {
        // GET: api/values
        public PS7andPS8FormReportInterface _ads;

        public PS7andPS8FormReportFacadeController(PS7andPS8FormReportInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public PFReportsDTO getinitialdata([FromBody]PFReportsDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        

        [Route("getEmployeedetailsBySelection")]
        public PFReportsDTO getEmployeedetailsBySelection([FromBody]PFReportsDTO dto)
        {
            return _ads.getEmployeedetailsBySelection(dto);
        }

        [Route("getdataps8")]
        public PFReportsDTO getdataps8([FromBody]PFReportsDTO dto)
        {
            return _ads.getdataps8(dto);
        }
    }
}
