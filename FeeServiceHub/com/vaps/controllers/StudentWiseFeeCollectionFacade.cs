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
    public class StudentWiseFeeCollectionFacade : Controller
    {
        public StudentWiseFeeCollectionInterface _feedefaulters;

        public StudentWiseFeeCollectionFacade(StudentWiseFeeCollectionInterface maspag)
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


        [Route("onchangeacademic")]
        public Task<CategoryWiseFeeCollectionDTO> onchangeacademic([FromBody]CategoryWiseFeeCollectionDTO temp)
        {
            return _feedefaulters.onchangeacademic(temp);
        }
        [Route("onselectclass")]
        public Task<CategoryWiseFeeCollectionDTO> onselectclass([FromBody]CategoryWiseFeeCollectionDTO temp)
        {
            return _feedefaulters.onselectclass(temp);
        }
        [Route("onselectsection")]
        public Task<CategoryWiseFeeCollectionDTO> onselectsection([FromBody]CategoryWiseFeeCollectionDTO temp)
        {
            return _feedefaulters.onselectsection(temp);
        }

    }
}
