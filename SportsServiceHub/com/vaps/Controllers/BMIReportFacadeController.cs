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
    public class BMIReportFacadeController : Controller
    {
        BMIReportInterface _interface;
        public BMIReportFacadeController(BMIReportInterface interfaces)
        {
            _interface = interfaces;
        }

        [Route("report")]
        public Task<BMICalculationDTO> report([FromBody]BMICalculationDTO data)
        {
            return _interface.report(data);
        }

        [Route("getDetails")]
        public BMICalculationDTO getDetails([FromBody]BMICalculationDTO data)
        {
            return _interface.getDetails(data);
        }

        [Route("get_class")]
        public BMICalculationDTO get_class([FromBody]BMICalculationDTO data)
        {
            return _interface.get_class(data);
        }

        [Route("get_section")]
        public BMICalculationDTO get_section([FromBody]BMICalculationDTO data)
        {
            return _interface.get_section(data);
        }


        [Route("getStudents")]
        public BMICalculationDTO getStudents([FromBody]BMICalculationDTO data)
        {
            return _interface.getStudents(data);
        }
        
    }
    
}
