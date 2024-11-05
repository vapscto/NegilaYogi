using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using PortalHub.com.vaps.HOD.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.HOD.Controllers
{
    [Route("api/[controller]")]
    public class HODStudentStrengthFacadeController : Controller
    {

        public HODStudentStrengthInterface _ChairmanDashboardReport;

        public HODStudentStrengthFacadeController(HODStudentStrengthInterface data)
        {
            _ChairmanDashboardReport = data;
        }
        [Route("Getdetails")]
        public ADMClassSectionStrengthDTO Getdetails([FromBody] ADMClassSectionStrengthDTO data)
        {


            return _ChairmanDashboardReport.Getdetails(data);

        }

        [Route("getclass")]
        public ADMClassSectionStrengthDTO getclass([FromBody] ADMClassSectionStrengthDTO data)
        {


            return _ChairmanDashboardReport.getclass(data);

        }
       
        [Route("Getsection")]
        public ADMClassSectionStrengthDTO Getsection([FromBody] ADMClassSectionStrengthDTO data)
        {


            return _ChairmanDashboardReport.Getsection(data);

        }
        
        [Route("Getsectioncount")]
        public ADMClassSectionStrengthDTO Getsectioncount([FromBody] ADMClassSectionStrengthDTO data)
        {


            return _ChairmanDashboardReport.Getsectioncount(data);

        }
    }
}
