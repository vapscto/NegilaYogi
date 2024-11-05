using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class RackReportFacde : Controller
    {
        public RackReportInterface _objInter;
        public RackReportFacde(RackReportInterface data)
        {
            _objInter = data;
        }
        
        [Route("getdetails")] 
        public RackReportDTO getdetails([FromBody]RackReportDTO data)
        {
            return _objInter.getdetails(data);
        }


        [Route("get_report")] 
        public Task<RackReportDTO> get_report([FromBody]RackReportDTO data)
        {
            return _objInter.get_report(data);
        }
    }
}
