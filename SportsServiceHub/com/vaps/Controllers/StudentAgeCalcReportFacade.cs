using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;
using SportsServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SportsServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class StudentAgeCalcReportFacade : Controller
    {
        // GET: api/<controller>

        public StudentAgeCalcReportInterface _ReportContext;

        public StudentAgeCalcReportFacade(StudentAgeCalcReportInterface dt)
        {
            _ReportContext = dt;
        }


        [Route("Getdetails")]
        public StudentAgeCalcReport_DTO Getdetails([FromBody]StudentAgeCalcReport_DTO data)//int IVRMM_Id
        {

            return _ReportContext.Getdetails(data);
        }


        [Route("showdetails")]
        public Task<StudentAgeCalcReport_DTO> showdetails([FromBody] StudentAgeCalcReport_DTO data)
        {
            return _ReportContext.showdetails(data);
        }

        [Route("get_class")]
        public StudentAgeCalcReport_DTO get_class([FromBody] StudentAgeCalcReport_DTO data)
        {
            return _ReportContext.get_class(data);
        }

        [Route("get_section")]
        public StudentAgeCalcReport_DTO get_section([FromBody] StudentAgeCalcReport_DTO data)
        {
            return _ReportContext.get_section(data);
        }

    }
}
