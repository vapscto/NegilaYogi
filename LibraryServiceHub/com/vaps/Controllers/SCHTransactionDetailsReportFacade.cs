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
    public class SCHTransactionDetailsReportFacade : Controller
    {
        // GET: api/<controller>
        public SCHTransactionDetailsReportInterface _objInter;
        public SCHTransactionDetailsReportFacade(SCHTransactionDetailsReportInterface para)
        {
            _objInter = para;
        }

        [Route("getdetails")]
        public SCHTransactionDetailsReportDTO getdetails([FromBody]SCHTransactionDetailsReportDTO id)
        {
            return _objInter.getdetails(id);
        }
        [Route("get_report")]
        public SCHTransactionDetailsReportDTO get_report([FromBody]SCHTransactionDetailsReportDTO id)
        {
            return _objInter.get_report(id);
        }
    }
}
