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
    public class MasterBoardandSchoolTypeFacade : Controller
    {
        public MasterBoardandSchoolTypeInterface _gen;

        public MasterBoardandSchoolTypeFacade(MasterBoardandSchoolTypeInterface mbf)
        {
            _gen = mbf;
        }

        [Route("getBoardDet/{id:int}")]
        public MasterBoardDTO getBoardDet(int id)
        {
            return _gen.getAllDetails(id);
        }

        [Route("getAllSchoolType")]
        public MasterSchoolTypeDTO GetAllSchoolType(MasterSchoolTypeDTO mstcat)
        {
            return _gen.getAllSchoolTypeDetails(mstcat);
        }
        [Route("getdetails/{id:int}")]

        public MasterBoardDTO getdet(int id)
        {
            // id = 12;
            return _gen.getdetails(id);
        }
        [Route("schoolTypeDet/{id:int}")]

        public MasterSchoolTypeDTO getschlTypedet(int id)
        {
            // id = 12;
            return _gen.getSchoolTypedetails(id);
        }
        // POST api/values
        [HttpPost]
        public MasterBoardDTO Post([FromBody]MasterBoardDTO cty)
        {
            // OrganisationDTO det = new OrganisationDTO();
            // det.IVRMMCT_Id = "45";
            return _gen.savedet(cty);
            // return det;
        }
        // POST api/values

        [Route("savedschlTypetails")]
        public MasterSchoolTypeDTO savedschlTypetails([FromBody]MasterSchoolTypeDTO cty)
        {
            // OrganisationDTO det = new OrganisationDTO();
            // det.IVRMMCT_Id = "45";
            return _gen.saveSchoolTypeDet(cty);
            // return det;
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
        public MasterBoardDTO Deleterec(int id)
        {
            return _gen.deleterec(id);
        }
        [HttpDelete]
        [Route("deleteSchoolTyperec/{id:int}")]
        public MasterSchoolTypeDTO deleteSchoolTyperec(int id)
        {
            return _gen.deleteSchoolTyperec(id);
        }
    }
}
