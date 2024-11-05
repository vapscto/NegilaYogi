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
    public class CategoryConcessionGroupMappingFacade : Controller
    {


        public CategoryConcessionGroupMappingInterface inter;

        public CategoryConcessionGroupMappingFacade(CategoryConcessionGroupMappingInterface u)
        {
            inter = u;
        }

        [Route("loaddata")]
        public CategoryConcessionGroupMappingDTO loaddata([FromBody] CategoryConcessionGroupMappingDTO data)
        {
            return inter.loaddata(data);
        }

        [Route("gethead")]
        public CategoryConcessionGroupMappingDTO gethead([FromBody] CategoryConcessionGroupMappingDTO data)
        {
            return inter.gethead(data);
        }
        [Route("getconcession")]
        public CategoryConcessionGroupMappingDTO getconcession([FromBody] CategoryConcessionGroupMappingDTO data)
        {
            return inter.getconcession(data);
        }
        [Route("save")]
        public CategoryConcessionGroupMappingDTO save([FromBody] CategoryConcessionGroupMappingDTO data)
        {
            return inter.save(data);
        }
        [Route("deactiveStudent")]
        public CategoryConcessionGroupMappingDTO deactiveStudent([FromBody] CategoryConcessionGroupMappingDTO data)
        {
            return inter.deactiveStudent(data);
        }
        [Route("EditData")]
        public CategoryConcessionGroupMappingDTO EditData([FromBody] CategoryConcessionGroupMappingDTO data)
        {
            return inter.EditData(data);
        } [Route("getgroup")]
        public CategoryConcessionGroupMappingDTO getgroup([FromBody] CategoryConcessionGroupMappingDTO data)
        {
            return inter.getgroup(data);
        }
    }
}
