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
    public class FeeRefundFacadeController : Controller
    {

        public FeeRefundInterface _feetar;

        public FeeRefundFacadeController(FeeRefundInterface maspag)
        {
            _feetar = maspag;
        }

        // GET api/values/5
   
        [Route("getalldetails")]
        public FeeRefundDTO getalldetails([FromBody] FeeRefundDTO id)
        {
            return _feetar.getalldetails(id);
        }


        [Route("getsection")]
        public FeeRefundDTO getsection([FromBody]FeeRefundDTO data)
        {
            return _feetar.getsection(data);
        }
        

        [Route("getstudent")]
        public FeeRefundDTO getstudent([FromBody]FeeRefundDTO data)
        {
            return _feetar.getstudent(data);
        }
        //getalldetails

        [HttpPost]
        [Route("radiobtndata")]
        public Task<FeeRefundDTO> radiobtndata([FromBody]FeeRefundDTO data)
        {
            return _feetar.getreport(data);
        }

    }
}
