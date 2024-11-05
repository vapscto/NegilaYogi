using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.admission;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeArrearRegisterReportFacadeController : Controller
    {
        public FeeArrearRegisterReportInterface _feetar;

        public FeeArrearRegisterReportFacadeController(FeeArrearRegisterReportInterface maspag)
        {
            _feetar = maspag;
        }

        // GET: api/values
   
        [HttpPost]
        [Route("getalldetails123")]
        public FeeArrearRegisterReportDTO Getdet([FromBody] FeeArrearRegisterReportDTO data)
        {
            return _feetar.getdata123(data);
        }


        [Route("getsection")]
        public FeeArrearRegisterReportDTO getsection([FromBody]FeeArrearRegisterReportDTO data)
        {
            return _feetar.getsection(data);
        }
        [Route("getstudent")]
        public FeeArrearRegisterReportDTO getstudent([FromBody]FeeArrearRegisterReportDTO data)
        {
            return _feetar.getstudent(data);
        }

        [Route("getgroupmappedheads")]
        public FeeArrearRegisterReportDTO getstuddetails([FromBody]FeeArrearRegisterReportDTO value)
        {
            return _feetar.getstuddet(value);
        }
        [Route("getreport")]
        public Task<FeeArrearRegisterReportDTO> getreport([FromBody] FeeArrearRegisterReportDTO data)
        {
            return _feetar.getreport(data);
        }
        [Route("get_groups")]
        public FeeArrearRegisterReportDTO get_groups([FromBody]FeeArrearRegisterReportDTO data)
        {
            return _feetar.get_groups(data);
        }
    }
}
