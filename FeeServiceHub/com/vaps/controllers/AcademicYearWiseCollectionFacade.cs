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
    public class AcademicYearWiseCollectionFacade : Controller
    {
        public AcademicYearWiseCollectionInterface _feedefaulters;

        public AcademicYearWiseCollectionFacade(AcademicYearWiseCollectionInterface maspag)
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
