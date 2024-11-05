using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using WebApplication1.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class AcademicFacadeController : Controller
    {
        public AcademicInterface _acd;
        public AcademicFacadeController(AcademicInterface acdm)
        {
            _acd = acdm;
        }
        [Route("getall")]
        public AcademicDTO Get([FromBody]AcademicDTO dto)
        {
            return _acd.getallDetails(dto);
        }


        [Route("getdetails/{id:int}")]
        public AcademicDTO getDetails(int id)
        {
            // id = 12;
            return _acd.getdetails(id);
        }
        [HttpPost]
        public AcademicDTO Post([FromBody]AcademicDTO acdm)
        {
            // OrganisationDTO det = new OrganisationDTO();
            // det.IVRMMCT_Id = "45";
            return _acd.saveProsdet(acdm);
            // return det;
        }
        // PUT api/values/5
        [HttpPut]
        public string Put([FromBody]OrganisationDTO org)
        {
            return "success";
        }

      
      
        [Route("deletedetails")]
        public AcademicDTO Deleterec([FromBody]AcademicDTO dto)
        {
            return _acd.deleterec(dto);
        }
       
        [HttpPost]
        [Route("deactivate")]
        public AcademicDTO deactivateAcdmYear([FromBody] AcademicDTO id)
        {
            // id = 12;
            return _acd.deactivate(id);
        }
        [Route("searchByColumn")]
        public AcademicDTO searchByColumn([FromBody] AcademicDTO dto)
        {
            // id = 12;
            return _acd.searchByColumn(dto);
        }
        [Route("saveorder")]
        public AcademicDTO saveorder([FromBody] AcademicDTO dto)
        {
            // id = 12;
            return _acd.saveorder(dto);
        }
        
    }
}
