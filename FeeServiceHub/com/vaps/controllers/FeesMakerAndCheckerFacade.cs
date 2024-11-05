using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeeServiceHub.com.vaps.interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeesMakerAndCheckerFacade : Controller
    {
        public FeesMakerAndCheckerInterface _feedefaulters;

        public FeesMakerAndCheckerFacade(FeesMakerAndCheckerInterface maspag)
        {
            _feedefaulters = maspag;
        }




        //  [HttpGet]
        [Route("getdetails")]
        public FeesMakerAndCheckerDTO getdetails([FromBody] FeesMakerAndCheckerDTO data)
        {
            return _feedefaulters.getdetails(data);
        }
        [HttpPost]
        [Route("Getreportdetails")]
        public FeesMakerAndCheckerDTO Getreportdetails([FromBody]FeesMakerAndCheckerDTO temp)
        {
            return _feedefaulters.Getreportdetails(temp);
        }
        [HttpPost]
        [Route("savedetails")]
        public FeesMakerAndCheckerDTO savedetails([FromBody]FeesMakerAndCheckerDTO temp)
        {
            return _feedefaulters.savedetails(temp);
        }


    }
}
