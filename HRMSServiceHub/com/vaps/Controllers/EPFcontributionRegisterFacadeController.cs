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
    public class EPFcontributionRegisterFacadeController : Controller
    {
        // GET: api/values
        public EPFcontributionRegisterInterface _ads;

        public EPFcontributionRegisterFacadeController(EPFcontributionRegisterInterface adstu)
        {
          _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public PFReportsDTO getinitialdata([FromBody]PFReportsDTO dto)
        {
          return _ads.getBasicData(dto);
        }

        //FilterEmployeeData
        [Route("FilterEmployeeData")]
        public PFReportsDTO FilterEmployeeData([FromBody]PFReportsDTO dto)
        {
          return _ads.FilterEmployeeData(dto);
        }

        [Route("getEmployeedetailsBySelection")]
        public PFReportsDTO getEmployeedetailsBySelection([FromBody]PFReportsDTO dto)
        {
          return _ads.getEmployeedetailsBySelection(dto);
        }

        [Route("getEmployeedetailsBySelectionBBKV")]
        public PFReportsDTO getEmployeedetailsBySelectionBBKV([FromBody]PFReportsDTO dto)
        {
            return _ads.getEmployeedetailsBySelectionBBKV(dto);
        }

        [Route("getEmployeedetailsBySelectionStJames")]
        public PFReportsDTO getEmployeedetailsBySelectionStJames([FromBody]PFReportsDTO dto)
        {
            return _ads.getEmployeedetailsBySelectionStJames(dto);
        }
    }
}
