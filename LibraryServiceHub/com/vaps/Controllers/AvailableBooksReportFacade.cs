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
    public class AvailableBooksReportFacade : Controller
    {
        // GET: api/<controller>
        public AvailableBooksReportInterface _objInter;
        public AvailableBooksReportFacade(AvailableBooksReportInterface para)
        {
            _objInter = para;
        }

        [Route("getdetails")]
        public AvailableBooksReport_DTO getdetails([FromBody]AvailableBooksReport_DTO id)
        {
            return _objInter.getdetails(id);
        }
        [Route("get_report")]
        public AvailableBooksReport_DTO get_report([FromBody]AvailableBooksReport_DTO id)
        {
            return _objInter.get_report(id);
        }
    }
}
