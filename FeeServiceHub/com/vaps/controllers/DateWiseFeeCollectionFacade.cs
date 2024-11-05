using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class DateWiseFeeCollectionFacade : Controller
    {
        public DateWiseFeeCollectionInterface _feedefaulters;

        public DateWiseFeeCollectionFacade(DateWiseFeeCollectionInterface maspag)
        {
            _feedefaulters = maspag;
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

        [HttpPost]
        [Route("onchangeacademic")]
        public Task<CategoryWiseFeeCollectionDTO> onchangeacademic([FromBody]CategoryWiseFeeCollectionDTO temp)
        {
            return _feedefaulters.onchangeacademic(temp);
        }
    }
}
