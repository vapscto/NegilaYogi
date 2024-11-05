using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeGroupGroupingFacadeController : Controller
    {
        FeeGroupGroupingInterface fggi;

        public FeeGroupGroupingFacadeController(FeeGroupGroupingInterface maspag)
        {
            fggi = maspag;
        }
      

        [HttpPost]
        [Route("SaveYearlyGrpdata/")]
        public FeeGroupMappingDTO SaveYearlyGrpdata([FromBody] FeeGroupMappingDTO reg)
        {
            return    fggi.SaveYearlyGroupData( reg);        
                    
        }

        // GET api/values/5
        [Route("getdetailsY/{id:int}")]
        public FeeGroupMappingDTO getorgdetY(int id)
        {
            return fggi.getdetailsY(id);
        }

        [HttpPost]
        [Route("deactivateY")]
        public FeeGroupMappingDTO deactivateY([FromBody] FeeGroupMappingDTO id)
        {
            // id = 12;
            return fggi.deactivateY(id);
        }

        [Route("getpagedetailsY/{id:int}")]
        //[Route("getenquirycontroller")]
        public FeeGroupMappingDTO getpagedetailsY(int id)
        {
            // id = 12;
            return fggi.getpageeditY(id);
        }

        [HttpDelete]
        [Route("deletedetailsY/{id:int}")]
        public FeeGroupMappingDTO DeleterecY(int id)
        {
            return fggi.deleterecY(id);
        }

    }
}
