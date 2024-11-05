using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.HOD.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.HOD;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.HOD.Controllers
{
    [Route("api/[controller]")]
    public class HODPaycareStaffDetailsFacade : Controller
    {

        public HODPaycareStaffDetailsInterface _HodPaycareStaffdetailsReport;

        public HODPaycareStaffDetailsFacade(HODPaycareStaffDetailsInterface data)
        {
            _HodPaycareStaffdetailsReport = data;
        }


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [Route("Getdetails")]
        public HODPaycareStaffDetails_DTO Getdetails([FromBody] HODPaycareStaffDetails_DTO data)//int IVRMM_Id
        {
            return _HodPaycareStaffdetailsReport.Getdetails(data);
        }
        

        [HttpPost]
        [Route("Getemppop")]
        public HODPaycareStaffDetails_DTO Getemppop([FromBody] HODPaycareStaffDetails_DTO data)
        {
            return _HodPaycareStaffdetailsReport.Getemppop(data);
        }

    }
}
