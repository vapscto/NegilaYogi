using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CollegeFeeService.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.College.Fee;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class CollegeOverallFeeStatusFacade : Controller
    {
        public CollegeOverallFeeStatusInterface objInterface;

        public CollegeOverallFeeStatusFacade(CollegeOverallFeeStatusInterface bdInterface)
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
        ////[HttpPost]
        [Route("get_report")]
        public CollegeOverallFeeStatusDTO get_report([FromBody] CollegeOverallFeeStatusDTO data)
        {
            return objInterface.get_report(data);
        }

        [Route("savedata")]
        public CollegeOverallFeeStatusDTO savedata([FromBody] CollegeOverallFeeStatusDTO data)
        {
            return objInterface.savedata(data);
        }

        [Route("editdata")]
        public CollegeOverallFeeStatusDTO editdata([FromBody]CollegeOverallFeeStatusDTO data)
        {
            return objInterface.editdata(data);
        }

        [Route("DeleteRecord")]
        public CollegeOverallFeeStatusDTO DeleteRecord([FromBody] CollegeOverallFeeStatusDTO data)
        {
            return objInterface.DeleteRecord(data);
        }
    }
}
