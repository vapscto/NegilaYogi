
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Chairman.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
//using AdmissionServiceHub.com.vaps.Interfaces;


using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Chairman.Controllers
{
    [Route("api/[controller]")]
    public class ADMCasteStrengthFacadeController : Controller
    {
        public ADMCasteStrengthInterface _ChairmanDashboardReport;

        public ADMCasteStrengthFacadeController(ADMCasteStrengthInterface data)
        {
            _ChairmanDashboardReport = data;
        }


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [Route("Getdetails")]
        public ADMCasteStrengthDTO Getdetails([FromBody] ADMCasteStrengthDTO data)
        {

           
            return  _ChairmanDashboardReport.Getdetails(data);
           
        }

        [HttpPost]
        [Route("getclass")]
        public ADMCasteStrengthDTO getclass([FromBody] ADMCasteStrengthDTO data)
        {


            return _ChairmanDashboardReport.getclass(data);

        }
        [HttpPost]
        [Route("Getsection")]
        public ADMCasteStrengthDTO Getsection([FromBody] ADMCasteStrengthDTO data)
        {


            return _ChairmanDashboardReport.Getsection(data);

        }
        [HttpPost]
        [Route("Getreport")]
        public ADMCasteStrengthDTO Getreport([FromBody] ADMCasteStrengthDTO data)
        {


            return _ChairmanDashboardReport.Getreport(data);

        }

        [HttpPost]
        [Route("Getstudentdetails")]
        public ADMCasteStrengthDTO Getstudentdetails([FromBody] ADMCasteStrengthDTO data)
        {


            return _ChairmanDashboardReport.Getstudentdetails(data);

        }
        



    }
}
