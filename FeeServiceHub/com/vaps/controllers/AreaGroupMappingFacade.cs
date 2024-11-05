using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.Transport;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class AreaGroupMappingFacade : Controller
    {
   

        public AreaGroupMappingInterface _feegroupHeadpage;

        public AreaGroupMappingFacade(AreaGroupMappingInterface maspag)
        {
            _feegroupHeadpage = maspag;
        }

        
        [Route("getdetails/{id:int}")]
        public AreaGroupMappingDTO getorgdet(int id)
        {
            return _feegroupHeadpage.getdetails(id);
        }

        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public AreaGroupMappingDTO getpagedetails(int id)
        {
            return _feegroupHeadpage.getpageedit(id);
        }
        // POST api/values
        [HttpPost]
        public AreaGroupMappingDTO Post([FromBody] AreaGroupMappingDTO org)
        {
           return _feegroupHeadpage.Savedetails(org);
        } 

        [HttpPost]
        [Route("deactivate")]
        public AreaGroupMappingDTO deactivate([FromBody] AreaGroupMappingDTO id)
        {
            return _feegroupHeadpage.deactivate(id);
        }


        [Route("savedataamount")]
        public TR_Area_AmountDTO savedataamount([FromBody]TR_Area_AmountDTO data)
        {
            return _feegroupHeadpage.savedataamount(data);
        }
        [Route("geteditdataamount")]
        public TR_Area_AmountDTO geteditdataamount([FromBody] TR_Area_AmountDTO data)
        {
            return _feegroupHeadpage.geteditdataamount(data);
        }
        [Route("activedeactiveamount")]
        public TR_Area_AmountDTO activedeactiveamount([FromBody] TR_Area_AmountDTO data)
        {
            return _feegroupHeadpage.activedeactiveamount(data);
        }
    }
}
