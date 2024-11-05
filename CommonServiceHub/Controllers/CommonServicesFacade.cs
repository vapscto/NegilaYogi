using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;
using System.Text;
using PreadmissionDTOs.com.vaps.Fees;
using CommonServiceHub.com.vaps.interfaces;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CommonServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]

    public class CommonServiceFacade : Controller
    {
        public CommonServiceInterface _org;
        public CommonServiceFacade(CommonServiceInterface orga)
        {
            _org = orga;
        }
        [HttpPost]
        [Route("getgroupmappedheads")]
        public FeeStudentTransactionDTO getstuddetails([FromBody]FeeStudentTransactionDTO value)
        {
            return _org.getstuddet(value);
        }
    }
}
