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
    public class ClgBookTransactionReportFacade : Controller
    {

        public ClgBookTransactionReportInterface _objInter;
        public ClgBookTransactionReportFacade(ClgBookTransactionReportInterface para)
        {
            _objInter = para;
        }

        [Route("getdetails")]
        public CLGBookTransactionDTO getdetails([FromBody] CLGBookTransactionDTO id)
        {
            return _objInter.getdetails(id);
        }

        [Route("get_report")]
        public Task<CLGBookTransactionDTO> get_report([FromBody] CLGBookTransactionDTO data)
        {
            return _objInter.get_report(data);
        }

    }
}
