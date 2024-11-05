using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeFeeService.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fee;

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class CLGStudentFeeEnablePartialPaymentFacadeController : Controller
    {
        public CLGStudentFeeEnablePartialPaymentInterface objInterface;

        public CLGStudentFeeEnablePartialPaymentFacadeController(CLGStudentFeeEnablePartialPaymentInterface bdInterface)
        {
            objInterface = bdInterface;
        }
        // GET: api/values

        [HttpGet]
        [Route("GetYearList/{id:int}")]
        public CollegeOverallFeeStatusDTO GetYearList(int id)
        {
            return objInterface.GetYearList(id);
        }
        [HttpPost]
        [Route("get_courses")]
        public CollegeOverallFeeStatusDTO get_courses([FromBody]CollegeOverallFeeStatusDTO id)
        {
            return objInterface.get_courses(id);
        }

        ////[HttpPost]
        [Route("get_branches")]
        public CollegeOverallFeeStatusDTO get_branches([FromBody]CollegeOverallFeeStatusDTO id)
        {
            return objInterface.get_branches(id);
        }
        ////[HttpPost]
        [Route("get_semisters")]
        public CollegeOverallFeeStatusDTO get_semisters([FromBody] CollegeOverallFeeStatusDTO data)
        {
            return objInterface.get_semisters(data);
        }
        [Route("get_student")]
        public CollegeOverallFeeStatusDTO get_student([FromBody] CollegeOverallFeeStatusDTO data)
        {
            return objInterface.get_student(data);
        }
        [Route("savedata")]
        public CollegeOverallFeeStatusDTO savedata([FromBody] CollegeOverallFeeStatusDTO data)
        {
            return objInterface.savedata(data);
        }
        [Route("deactivate")]
        public CollegeOverallFeeStatusDTO deactivate([FromBody] CollegeOverallFeeStatusDTO data)
        {
            return objInterface.deactivate(data);
        }
    }
}