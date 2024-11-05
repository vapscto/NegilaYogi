using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeDefaulterReportFacade : Controller
    {


        public FeeDefaulterReportInterface _feedefaulters;

        public FeeDefaulterReportFacade(FeeDefaulterReportInterface maspag)
        {
            _feedefaulters = maspag;
        }


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }




        [Route("getdetails")]
        public FeeTransactionPaymentDTO getorgdet([FromBody] FeeTransactionPaymentDTO data)
        {
            return _feedefaulters.getdetails(data);
        }




        [HttpPost]
        [Route("getgrpterms")]
        public FeeTransactionPaymentDTO getgrpterms([FromBody]FeeTransactionPaymentDTO temp)
        {
            return _feedefaulters.getgrpterms(temp);
        }

        [HttpPost]
        [Route("radiobtndata")]
        public Task<FeeTransactionPaymentDTO> radiobtndata([FromBody]FeeTransactionPaymentDTO temp)
        {
            return _feedefaulters.getradiofiltereddata(temp);
        }



        [Route("get_groups")]
        public FeeTransactionPaymentDTO get_groups([FromBody]FeeTransactionPaymentDTO temp)
        {
            return _feedefaulters.get_groups(temp);
        }


        [Route("ExportToExcle/")]

        public string ExportToExcle([FromBody] FeeTransactionPaymentDTO reg)
        {
            string str = "";
            return str;
        }

        [Route("getsection")]
        public FeeTransactionPaymentDTO getsection([FromBody]FeeTransactionPaymentDTO data)
        {
            return _feedefaulters.getsection(data);
        }
        [Route("sendsms")]
        public Task<FeetransactionSMS> sendsms([FromBody]FeetransactionSMS data)
        {
            return _feedefaulters.sendsms(data);
        }
        [Route("sendemail")]
        public FeeTransactionPaymentDTO sendemail([FromBody]FeeTransactionPaymentDTO data)
        {
            return _feedefaulters.sendemail(data);
        }
        //======================================Staff Portal

        [Route("getstaffwiseclass")]
        public Task<FeeTransactionPaymentDTO> getstaffwiseclass([FromBody]FeeTransactionPaymentDTO temp)
        {
            return _feedefaulters.getstaffwiseclass(temp);
        }

        [HttpPost]
        [Route("getStaffterms")]
        public FeeTransactionPaymentDTO getStaffterms([FromBody]FeeTransactionPaymentDTO temp)
        {
            return _feedefaulters.getStaffterms(temp);
        }
        [Route("saveremark")]
        public FeeTransactionPaymentDTO saveremark([FromBody]FeeTransactionPaymentDTO temp)
        {
            return _feedefaulters.saveremark(temp);
        }
        [Route("feeremarkreport")]
        public FeeTransactionPaymentDTO feeremarkreport([FromBody]FeeTransactionPaymentDTO temp)
        {
            return _feedefaulters.feeremarkreport(temp);
        }
        [Route("feeremarkload")]
        public FeeTransactionPaymentDTO feeremarkload([FromBody]FeeTransactionPaymentDTO temp)
        {
            return _feedefaulters.feeremarkload(temp);
        }
        [Route("feeremarksection")]
        public FeeTransactionPaymentDTO feeremarksection([FromBody]FeeTransactionPaymentDTO temp)
        {
            return _feedefaulters.feeremarksection(temp);
        }

        [Route("feedefaultersmstriggering")]
        public FeeTransactionPaymentDTO feedefaultersmstriggering([FromBody]FeeTransactionPaymentDTO temp)
        {
            return _feedefaulters.feedefaultersmstriggering(temp);
        }


    }
}
