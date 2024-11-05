
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
    public class ExmSectionPerformFacadeController : Controller
    {
        public ExmSectionPerformInterface _ChairmanDashboardReport;

        public ExmSectionPerformFacadeController(ExmSectionPerformInterface data)
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
        public ExmSectionPerformDTO Getdetails([FromBody] ExmSectionPerformDTO data)
        {

           
            return  _ChairmanDashboardReport.Getdetails(data);
           
        }

        [HttpPost]
        [Route("getcategory")]
        public ExmSectionPerformDTO getclass([FromBody] ExmSectionPerformDTO data)
        {


            return _ChairmanDashboardReport.getcategory(data);

        }
        [HttpPost]
        [Route("getclassexam")]
        public ExmSectionPerformDTO getclassexam([FromBody] ExmSectionPerformDTO data)
        {


            return _ChairmanDashboardReport.getclassexam(data);

        }
        [HttpPost]
        [Route("showreport")]
        public ExmSectionPerformDTO showreport([FromBody] ExmSectionPerformDTO data)
        {


            return _ChairmanDashboardReport.showreport(data);

        }

        [HttpPost]
        [Route("getsubject")]
        public ExmSectionPerformDTO getsubject([FromBody] ExmSectionPerformDTO data)
        {


            return _ChairmanDashboardReport.getsubject(data);

        }



        

    }
}
