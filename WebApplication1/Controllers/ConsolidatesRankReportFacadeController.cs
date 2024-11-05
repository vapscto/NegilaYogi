using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using PreadmissionDTOs;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class ConsolidatesRankReportFacadeController : Controller
    {
        public ConsolidatesRankReportInterface _MasterModule;

        public ConsolidatesRankReportFacadeController(ConsolidatesRankReportInterface MasterModule)
        {
            _MasterModule = MasterModule;
        }
        [HttpPost]
        [Route("Getdetails")]
        public WrittenTestMarksBindDataDTO Getdetails([FromBody] WrittenTestMarksBindDataDTO data)
        {
           
            return _MasterModule.Getdetails(data);
        }
        [HttpPost]
        [Route("getclass")]
        public WrittenTestMarksBindDataDTO getclass([FromBody] WrittenTestMarksBindDataDTO data)
        {


            return _MasterModule.getclass(data);

        }
        [HttpPost]
        [Route("Getreport")]
        public WrittenTestMarksBindDataDTO Getreport([FromBody] WrittenTestMarksBindDataDTO data)
        {
            return _MasterModule.Getreport(data);

        }

    }
}
