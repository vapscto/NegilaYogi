using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CollegeFeeService.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.College.Fee;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class CollegeFeeDetailsfacade : Controller
    {
        public CollegeFeeDetailsInterface objInterface;

        public CollegeFeeDetailsfacade(CollegeFeeDetailsInterface bdInterface)
        {
            objInterface = bdInterface;
        }
        // GET: api/values

        [HttpGet]
        [Route("GetYearList/{id:int}")]
        public CollegeStudentLedgerDTO GetYearList(int id)
        {
            return objInterface.GetYearList(id);
        }

        [HttpPost]
        [Route("get_courses")]
        public CollegeStudentLedgerDTO get_courses([FromBody]CollegeStudentLedgerDTO id)
        {
            return objInterface.get_courses(id);
        }

        ////[HttpPost]
        [Route("get_branches")]
        public CollegeStudentLedgerDTO get_branches([FromBody]CollegeStudentLedgerDTO id)
        {
            return objInterface.get_branches(id);
        }
        ////[HttpPost]
        [Route("get_semisters")]
        public CollegeStudentLedgerDTO get_semisters([FromBody] CollegeStudentLedgerDTO data)
        {
            return objInterface.get_semisters(data);
        }

        [Route("get_report")]
        public Task<CollegeStudentLedgerDTO> get_report([FromBody] CollegeStudentLedgerDTO data)
        {
            return objInterface.get_report(data);
        }

        

  [Route("get_concession_report")]
        public Task<CollegeStudentLedgerDTO> get_concession_report([FromBody] CollegeStudentLedgerDTO data)
        {
            return objInterface.get_concession_report(data);
        }


    }
}
