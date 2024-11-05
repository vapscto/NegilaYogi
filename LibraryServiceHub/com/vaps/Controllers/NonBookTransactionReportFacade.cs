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
    public class NonBookTransactionReportFacade : Controller
    {

        public NonBookTransactionReportInterface _objInter;
        public NonBookTransactionReportFacade(NonBookTransactionReportInterface para)
        {
            _objInter = para;
        }

        [Route("getdetails")]
        public NonBookReport_DTO getdetails([FromBody] NonBookReport_DTO id)
        {
            return _objInter.getdetails(id);
        }

        [Route("get_report")]
        public Task<NonBookReport_DTO> get_report([FromBody] NonBookReport_DTO data)
        {
            return _objInter.get_report(data);
        }



    }
}
