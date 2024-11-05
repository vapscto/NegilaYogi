
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
    public class ExamHomeFacadeController : Controller
    {
        public ExamHomeInterface _ChairmanDashboardReport;

        public ExamHomeFacadeController(ExamHomeInterface data)
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
        public ExamHomeDTO Getdetails([FromBody] ExamHomeDTO data)
        {

           
            return  _ChairmanDashboardReport.Getdetails(data);
           
        }

        [HttpPost]
        [Route("getcategory")]
        public ExamHomeDTO getclass([FromBody] ExamHomeDTO data)
        {


            return _ChairmanDashboardReport.getcategory(data);

        }
        [HttpPost]
        [Route("getclassexam")]
        public ExamHomeDTO getclassexam([FromBody] ExamHomeDTO data)
        {


            return _ChairmanDashboardReport.getclassexam(data);

        }
        [HttpPost]
        [Route("showreport")]
        public ExamHomeDTO showreport([FromBody] ExamHomeDTO data)
        {


            return _ChairmanDashboardReport.showreport(data);

        }

        [HttpPost]
        [Route("showsectioncount")]
        public ExamHomeDTO showsectioncount([FromBody] ExamHomeDTO data)
        {


            return _ChairmanDashboardReport.showsectioncount(data);

        }



        

    }
}
