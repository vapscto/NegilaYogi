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
    public class Clg_StudentFeeGroupMappingFacade : Controller
    {
        public Clg_StudentFeeGroupMappingInterface objInterface;

        public Clg_StudentFeeGroupMappingFacade (Clg_StudentFeeGroupMappingInterface bdInterface)
        {
            objInterface = bdInterface;
        }
        // GET: api/values

        [HttpGet]
        [Route("GetYearList/{id:int}")]
        public Clg_StudentFeeGroupMapping_DTO GetYearList(int id)
        {
            return objInterface.GetYearList(id);
        }

        [HttpPost]
        [Route("get_courses")]
        public Clg_StudentFeeGroupMapping_DTO get_courses([FromBody]Clg_StudentFeeGroupMapping_DTO id)
        {
            return objInterface.get_courses(id);
        }

        ////[HttpPost]
        [Route("get_branches")]
        public Clg_StudentFeeGroupMapping_DTO get_branches([FromBody]Clg_StudentFeeGroupMapping_DTO id)
        {
            return objInterface.get_branches(id);
        }
        ////[HttpPost]
        [Route("get_semisters")]
        public Clg_StudentFeeGroupMapping_DTO get_semisters([FromBody] Clg_StudentFeeGroupMapping_DTO data)
        {
            return objInterface.get_semisters(data);
        }
        ////[HttpPost]
        [Route("get_report")]
        public Clg_StudentFeeGroupMapping_DTO get_report([FromBody] Clg_StudentFeeGroupMapping_DTO data)
        {
            return objInterface.get_report(data);
        }

        [Route("savedata")]
        public Clg_StudentFeeGroupMapping_DTO savedata([FromBody] Clg_StudentFeeGroupMapping_DTO data)
        {
            return objInterface.savedata(data);
        }

        [Route("editdata")]
        public Clg_StudentFeeGroupMapping_DTO editdata([FromBody]Clg_StudentFeeGroupMapping_DTO data)
        {
            return objInterface.editdata(data);
        }

        [Route("DeleteRecord")]
        public Clg_StudentFeeGroupMapping_DTO DeleteRecord([FromBody] Clg_StudentFeeGroupMapping_DTO data)
        {
            return objInterface.DeleteRecord(data);
        }

        //saveeditdata
        [Route("saveeditdata")]
        public Clg_StudentFeeGroupMapping_DTO saveeditdata([FromBody] Clg_StudentFeeGroupMapping_DTO data)
        {
            return objInterface.saveeditdata(data);
        }
    }
}
