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
    public class DefaulterFeeCollectionGraphFacade : Controller
    {
        public DefaulterFeeCollectionGraphInterface _feedefaulters;

        public DefaulterFeeCollectionGraphFacade(DefaulterFeeCollectionGraphInterface maspag)
        {
            _feedefaulters = maspag;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getdetails")]
        public CategoryWiseFeeCollectionDTO getdetails([FromBody]CategoryWiseFeeCollectionDTO data)
        {
            return _feedefaulters.getdetails(data);
        }




        [HttpPost]
        [Route("radiobtndata")]
        public Task<CategoryWiseFeeCollectionDTO> radiobtndata([FromBody]CategoryWiseFeeCollectionDTO temp)
        {
            return _feedefaulters.radiobtndata(temp);
        }
    }
}
