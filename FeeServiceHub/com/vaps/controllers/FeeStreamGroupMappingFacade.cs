using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.admission;

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeStreamGroupMappingFacade : Controller
    {
        FeeStreamGroupMappingInterface _inter;
        public FeeStreamGroupMappingFacade(FeeStreamGroupMappingInterface inter)
        {
            _inter = inter;
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
             

        [Route("getData")]
        public FeeStreamGroupMappingDTO getData([FromBody] FeeStreamGroupMappingDTO data)
        {
            return _inter.getData(data);
        }

        [Route("saveData")]
        public FeeStreamGroupMappingDTO savedata([FromBody] FeeStreamGroupMappingDTO data)
        {
            return _inter.saveData(data);
        }

        [HttpPost]
        [Route("deactivate")]
        public FeeStreamGroupMappingDTO deactivate([FromBody] FeeStreamGroupMappingDTO id)
        {
            // id = 12;
            return _inter.deactivate(id);
        }


        [Route("Editdetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public FeeStreamGroupMappingDTO Editdetails(int id)
        {
            // id = 12;
            return _inter.Editdetails(id);
        }



    }
}
