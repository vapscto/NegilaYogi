using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using PreadmissionDTOs;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class MasterCategoryFacade : Controller
    {
        public MasterCategoryInterface _catry;

        public MasterCategoryFacade(MasterCategoryInterface ct)
        {
            _catry = ct;
        }

        [Route("getdata/{id:int}")]
        public MasterCategoryDTO Get(int id)
        {
            return _catry.getAllDetails(id);
        }

       
        [Route("getdetails")]      
        public MasterCategoryDTO getcatgrydet([FromBody]MasterCategoryDTO id)
        {
            return _catry.getdetails(id);
        }
        
        // POST api/values
        [HttpPost]
        public MasterCategoryDTO Post([FromBody]MasterCategoryDTO cty)
        {
            return _catry.saveCategorydet(cty);          
        }

        // PUT api/values/5
        [HttpPut]
        public string Put([FromBody]OrganisationDTO org)
        {
            return "success";
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public MasterCategoryDTO Deleterec(int id)
        {
            return _catry.deleterec(id);
        }

        [HttpPost]
        [Route("deactivate")]
        public MasterCategoryDTO deactivate([FromBody] MasterCategoryDTO data)
        {
            // id = 12;
            return _catry.deactivate(data);
        }

    }
}
