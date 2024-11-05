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
    public class BookTypeReportFacade : Controller
    {

        public BookTypeReportInterface _objInter;

        public BookTypeReportFacade(BookTypeReportInterface para)
        {
            _objInter = para; 
        }
        [Route("get_report")]
        public BookTypeReportDTO get_report([FromBody]BookTypeReportDTO data)
        {
           
            return _objInter.get_report(data);
        }
    }
}
