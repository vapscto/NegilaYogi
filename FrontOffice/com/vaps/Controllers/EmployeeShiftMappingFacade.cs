using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;
using FrontOfficeHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FrontOfficeHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeShiftMappingFacade : Controller
    {
        public EmployeeShiftMappingInterface _ttbreaktime;


        public EmployeeShiftMappingFacade(EmployeeShiftMappingInterface maspag)
        {
            _ttbreaktime = maspag;
        }


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        // GET api/values/5
        [Route("getdetails/{id:int}")]
        public EmployeeShiftMappingDTO getpagedetails(int id)
        {
            return _ttbreaktime.getdetails(id);
        }


        [Route("Shiftname")]
        public EmployeeShiftMappingDTO Shiftname([FromBody]EmployeeShiftMappingDTO data)
        {
            return _ttbreaktime.Shiftname(data);
        }


        [HttpPost]
        [Route("savedetail")]
        public EmployeeShiftMappingDTO savedetail([FromBody] EmployeeShiftMappingDTO org)
        {
            return _ttbreaktime.savedetail(org);
        }


        [Route("editdetails/{id:int}")]
        public EmployeeShiftMappingDTO editdetails(int id)
        {
            return _ttbreaktime.editdetails(id);
        }

        //[Route("getpagedetails/{id:int}")]
        ////[Route("getenquirycontroller")]
        //public EmployeeShiftMappingDTO getordetails(int id)
        //{
        //    // id = 12;
        //    return _ttbreaktime.getpageedit(id);
        //}
        //[HttpDelete]
        [Route("deletedetails")]
        public EmployeeShiftMappingDTO Deleterec([FromBody] EmployeeShiftMappingDTO data)
        {
            return _ttbreaktime.deleterec(data);
        }


        [Route("get_departments")]
        public EmployeeShiftMappingDTO get_departments([FromBody] EmployeeShiftMappingDTO data)
        {
            return _ttbreaktime.get_departments(data);
        }

        [Route("get_designation")]
        public EmployeeShiftMappingDTO get_designation([FromBody] EmployeeShiftMappingDTO data)
        {
            return _ttbreaktime.get_designation(data);
        }

        [Route("get_employee")]
        public EmployeeShiftMappingDTO get_employee([FromBody] EmployeeShiftMappingDTO data)
        {
            return _ttbreaktime.get_employee(data);
        }
    }
}
