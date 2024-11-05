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
    public class BookRegisterReportFacade : Controller
    {
        public BookRegisterReportInterface _objInter;
        public BookRegisterReportFacade(BookRegisterReportInterface para)
        {
            _objInter = para;
        }

        [Route("getdetails/{id:int}")]
        public BookRegisterReportDTO getdetails(int id)
        {
            return _objInter.getdetails(id);
        }
        [Route("get_report")]
        public BookRegisterReportDTO get_report([FromBody]BookRegisterReportDTO id)
        {
            return _objInter.get_report(id);
        }
        //BarCode
        [Route("BarCode")]
        public BookRegisterReportDTO BarCode([FromBody]BookRegisterReportDTO id)
        {
            return _objInter.BarCode(id);
        }
    }
}
