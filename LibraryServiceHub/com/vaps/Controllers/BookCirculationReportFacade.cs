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
    public class BookCirculationReportFacade : Controller
    {
        public BookCirculationReportInterface _objInter;
        public BookCirculationReportFacade(BookCirculationReportInterface para)
        {
            _objInter = para;
        }

        [Route("getdetails")]
        public BookCirculationReportDTO getdetails([FromBody] BookCirculationReportDTO id)
        {
            return _objInter.getdetails(id);
        }

        [Route("getstuddetails")]
        public BookCirculationReportDTO getstuddetails([FromBody] BookCirculationReportDTO data)
        {
            return _objInter.getstuddetails(data);
        }
        [Route("get_report")]
        public Task<BookCirculationReportDTO> get_report([FromBody] BookCirculationReportDTO data)
        {
            return _objInter.get_report(data);
        }
    }
}
