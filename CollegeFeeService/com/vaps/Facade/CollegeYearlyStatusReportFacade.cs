using CollegeFeeService.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fee;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class CollegeYearlyStatusReportFacade : Controller
    {
        public CollegeYearlyStatusReportInterface objInterface;

        public CollegeYearlyStatusReportFacade(CollegeYearlyStatusReportInterface bdInterface)
        {
            objInterface = bdInterface;
        }

        // GET: api/values

        [HttpGet]
        [Route("GetYearList/{id:int}")]
        public CollegeYearlyStatusReportDTO GetYearList(int id)
        {
            return objInterface.GetYearList(id);
        }

        [Route("savedata")]
        public CollegeYearlyStatusReportDTO savedata([FromBody] CollegeYearlyStatusReportDTO data)
        {
            return objInterface.savedata(data);
        }

        [Route("get_group")]
        public CollegeYearlyStatusReportDTO get_group([FromBody] CollegeYearlyStatusReportDTO data)
        {
            return objInterface.get_group(data);
        }
    }
}