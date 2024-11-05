using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using TimeTableServiceHub.Interfaces;
using PreadmissionDTOs.com.vaps.TT;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeTableServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class CategoryClassMappFacadeController : Controller
    {
        public CategoryClassMappInterface _acd;
        public CategoryClassMappFacadeController(CategoryClassMappInterface acdm)
        {
            _acd = acdm;
        }
        [HttpPost]
        [Route("getall")]
        public TT_Category_Class_DTO getall([FromBody]TT_Category_Class_DTO dto)
        {
            return _acd.getallDetails(dto);
        }

        [Route("getdetails/")]
        public TT_Category_Class_DTO getDetails([FromBody]TT_Category_Class_DTO id)
        {
            // id = 12;
            return _acd.getdetails(id);
        }
        [HttpPost]
        public TT_Category_Class_DTO Post([FromBody]TT_Category_Class_DTO acdm)
        {
            // OrganisationDTO det = new OrganisationDTO();
            // det.IVRMMCT_Id = "45";
            return _acd.saveProsdet(acdm);
            // return det;
        }
        //// PUT api/values/5
        [HttpPut]
        public string Put([FromBody]OrganisationDTO org)
        {
            return "success";
        }



        [Route("deletedetails")]
        public TT_Category_Class_DTO Deleterec([FromBody]TT_Category_Class_DTO dto)
        {
            return _acd.deleterec(dto);
        }

        //[HttpPost]
        //[Route("deactivate")]
        //public AcademicDTO deactivateAcdmYear([FromBody] AcademicDTO id)
        //{
        //    // id = 12;
        //    return _acd.deactivate(id);
        //}
        //[Route("searchByColumn")]
        //public AcademicDTO searchByColumn([FromBody] AcademicDTO dto)
        //{
        //    // id = 12;
        //    return _acd.searchByColumn(dto);
        //}
    }
}
