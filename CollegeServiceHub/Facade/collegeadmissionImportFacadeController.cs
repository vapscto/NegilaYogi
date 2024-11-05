using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeServiceHub.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class collegeadmissionImportFacadeController : Controller
    {
        public collegeadmissionImportInterface _ads;

        public collegeadmissionImportFacadeController(collegeadmissionImportInterface adstu)
        {
            _ads = adstu;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [Route("savedata")]
        public Task<CollegeImportStudentWrapperDTO> getinitialdata([FromBody] CollegeImportStudentWrapperDTO stud)
        {
            return _ads.getdetails(stud);
        }
        [Route("checkvalidation")]
        public Task<CollegeImportStudentWrapperDTO> checkvalidation([FromBody] CollegeImportStudentWrapperDTO data)
        {
            return _ads.checkvalidation(data);
        }

    }
}
