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
    public class LibTransactionReportFacade : Controller
    {

        public LibTransactionReportInterface _objInter;

        public LibTransactionReportFacade(LibTransactionReportInterface para)
        {
            _objInter = para; 
        }


        [Route("getdetails")]
        public LibTransactionReportDTO getdetails([FromBody]LibTransactionReportDTO data)
        {
            return _objInter.getdetails(data);
        }

        [Route("get_report")]
        public LibTransactionReportDTO get_report([FromBody]LibTransactionReportDTO data)
        {
           
            return _objInter.get_report(data);
        }
        [Route("CLGget_report")]
        public LibTransactionReportDTO CLGget_report([FromBody]LibTransactionReportDTO data)
        {
           
            return _objInter.CLGget_report(data);
        }
    }
}
